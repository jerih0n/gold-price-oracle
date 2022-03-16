using GoldPriceOracle.Configuration;
using GoldPriceOracle.Infrastructure.Background;
using GoldPriceOracle.Infrastructure.Integration.Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace GoldPriceOracle.Node.Extensions
{
    public static class HostBuilderExtenstions
    {
        public static IHostBuilder SubscribeForBlockchainEvents(this IHostBuilder hostBuilder, IConfigurationRoot config)
        {
            var blockchainData = GetBlockchainNetworkOptions(config);
            var applicationUrl = GetApplicationUrl(config);

            INodeLogger nodeLogger = Activator.CreateInstance<NodeLogger>();
            var blockchainEventListener = (BlockchainEventListener)Activator.CreateInstance(typeof(BlockchainEventListener), blockchainData, applicationUrl, nodeLogger);

            //Price Aggregators Events
            Task.Factory.StartNew(() => blockchainEventListener.SubscriteForNewPriceRoundVoteEvent(GetGoldPriceResolverAddress(config)));

            //Timer
            Task.Factory.StartNew(() => blockchainEventListener.SubscribeForNewEraElectionEvent(GetTimerAddress(config)));
            Task.Factory.StartNew(() => blockchainEventListener.SubscribeForNewPriceRoundEvent(GetTimerAddress(config)));

            //PoS events
            Task.Factory.StartNew(() => blockchainEventListener.SubscribeForNewEraProposalEvent(GetGoldPriceOracleTokenAddress(config)));
            Task.Factory.StartNew(() => blockchainEventListener.SybscribeForNewEraElectionComplitedEvent(GetGoldPriceOracleTokenAddress(config)));
            Task.Factory.StartNew(() => blockchainEventListener.SubscribeForEndEraByNewElectedChairman(GetGoldPriceOracleTokenAddress(config)));

            return hostBuilder;
        }

        private static BlockchainNetworkOptions GetBlockchainNetworkOptions(IConfigurationRoot config)
        {
            var blockchainData = config.GetSection("Blockchain:BlockchainNetwork");

            var blockchainNetworkOptions = new BlockchainNetworkOptions
            {
                NetworkId = blockchainData.GetValue<int>("NetworkId"),
                Port = blockchainData.GetValue<int>("Port"),
                RPCUrl = blockchainData.GetValue<string>("RPCUrl"),
                WebsocketUrl = blockchainData.GetValue<string>("WebsocketUrl")
            };

            return blockchainNetworkOptions;
        }

        private static string GetGoldPriceResolverAddress(IConfigurationRoot config)
             => config.GetSection("Blockchain:SmartContracts:GoldPriceResolver").GetValue<string>("Address");

        private static string GetTimerAddress(IConfigurationRoot config)
            => config.GetSection("Blockchain:SmartContracts:Timer").GetValue<string>("Address");

        private static object GetApplicationUrl(IConfigurationRoot config)
            => config.GetValue<string>("ApplicationUrl");

        private static string GetGoldPriceOracleTokenAddress(IConfiguration config)
            => config.GetSection("Blockchain:SmartContracts:GoldPriceOracleERC20Token").GetValue<string>("Address");
    }
}