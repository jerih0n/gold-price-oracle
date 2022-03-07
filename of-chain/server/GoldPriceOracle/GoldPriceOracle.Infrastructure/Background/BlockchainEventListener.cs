using GoldPriceOracle.Configuration;
using GoldPriceOracle.Connection.Blockchain.Contracts.GoldPriceResolver;
using GoldPriceOracle.Connection.Blockchain.Contracts.Timer;
using GoldPriceOracle.Infrastructure.Background.Requests;
using GoldPriceOracle.Infrastructure.Utils;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.JsonRpc.WebSocketStreamingClient;
using Nethereum.RPC.Reactive.Eth.Subscriptions;
using Nethereum.Web3;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoldPriceOracle.Infrastructure.Background
{
    public sealed class BlockchainEventListener
    {
        private readonly HttpClient _httpClinet;
        private readonly BlockchainNetworkOptions _options;
        private readonly string _httpUrl;
        private readonly string _webSocketUrl;
        private readonly Web3 _web3;
        private readonly string _applicationUrl;

        public BlockchainEventListener(BlockchainNetworkOptions options, string applicationUrl)
        {
            _options = options;

            _httpUrl = $"{_options.RPCUrl}:{_options.Port}";
            _webSocketUrl = $"{_options.WebsocketUrl}:{_options.Port}";
            _web3 = new Web3(_httpUrl);
            _httpClinet = new HttpClient();
            _applicationUrl = applicationUrl;
        }

        public async Task SubscriteForNewPriceRoundVoteEvent(string contractAddress)
           => await SybscribeForEvent<NewPriceVoteEventDTO>(contractAddress, HandleNewPriceRoundEvent);

        public async Task SubscribeForNewEraEvent(string contractAddress)
          => await SybscribeForEvent<StartNewEraEventDTO>(contractAddress, HandleStartNewEraEvent);

        public async Task SubscribeForNewPriceRoundEvent(string contractAddress)
          => await SybscribeForEvent<StarNewPriceRoundEventDTO>(contractAddress, HandleStartNewPriceRoundEvent);

        private async Task SybscribeForEvent<TEvent>(string contractAddress, Func<TEvent, Task> action) where TEvent : IEventDTO, new()
        {
            using (var client = new StreamingWebSocketClient(_webSocketUrl))
            {
                // create a log filter specific to Transfers
                // this filter will match any Transfer (matching the signature) regardless of address
                var filterTransfers = _web3.Eth.GetEvent<NewPriceVoteEventDTO>(contractAddress).CreateFilterInput();

                // create the subscription
                // it won't do anything yet
                var subscription = new EthLogsObservableSubscription(client);

                subscription.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
                {
                    try
                    {
                        // decode the log into a typed event log
                        var decoded = Event<TEvent>.DecodeEvent(log);
                        action(decoded.Event);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Log Address: " + log.Address + " is not a standard transfer log:", ex.Message);
                    }
                });

                // open the web socket connection
                await client.StartAsync();

                // begin receiving subscription data
                // data will be received on a background thread
                await subscription.SubscribeAsync(filterTransfers);

                while (true)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private async Task HandleNewPriceRoundEvent(NewPriceVoteEventDTO newPriceVoteEventDTO)
        {
            var requst = new NewPriceRoundVoteRequest(newPriceVoteEventDTO.AssetSymbol,
                newPriceVoteEventDTO.CurrencySymbol,
                newPriceVoteEventDTO.RoundId.ToHex(),
                newPriceVoteEventDTO.ProposalEmiterAddress,
                newPriceVoteEventDTO.Price.ToString()
                );

            await SendInternalHttpPostRequestAsync(requst, "price-round-vote");
        }

        private async Task HandleStartNewEraEvent(StartNewEraEventDTO startNewEraEventDTO)
        {
            var request = new StarNewEraEventRequest(startNewEraEventDTO.NewEraId_.ToString(), startNewEraEventDTO.NewEraId_.ToHex());
            await SendInternalHttpPostRequestAsync(request, "new-era-start");
        }

        private async Task HandleStartNewPriceRoundEvent(StarNewPriceRoundEventDTO starNewPriceRoundEventDTO)
            => await SendInternalHttpPostRequestAsync(new StartNewPriceRoundRequest(starNewPriceRoundEventDTO.UtcTimeStamp.ToString()), "start-new-price-round");

        private async Task SendInternalHttpPostRequestAsync(object requst, string endpoint)
        {
            var requestJson = JsonConvert.SerializeObject(requst);
            var stringContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_applicationUrl}/internal/{endpoint}")
            {
                Content = stringContent
            };

            await _httpClinet.SendAsync(request);
        }
    }
}