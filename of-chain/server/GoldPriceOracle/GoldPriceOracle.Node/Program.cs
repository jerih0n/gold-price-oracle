using GoldPriceOracle.Configuration;
using GoldPriceOracle.Infrastructure.Background;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace GoldPriceOracle.Node
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var blockchainData = GetBlockchainNetworkOptions(config);
            var applicationUrl = GetApplicationUrl(config);
            var blockchainEventListener = (BlockchainEventListener)Activator.CreateInstance(typeof(BlockchainEventListener), blockchainData, applicationUrl);

            //Price Aggregators Events
            Task.Factory.StartNew(() => blockchainEventListener.SubscriteForNewPriceRoundVoteEvent(GetGoldPriceResolverAddress(config)));

            //Timer
            Task.Factory.StartNew(() => blockchainEventListener.SubscribeForNewEraElectionEvent(GetTimerAddress(config)));
            Task.Factory.StartNew(() => blockchainEventListener.SubscribeForNewPriceRoundEvent(GetTimerAddress(config)));

            //PoS events
            Task.Factory.StartNew(() => blockchainEventListener.SubscribeForNewEraProposalEvent(GetGoldPriceOracleTokenAddress(config)));
            Task.Factory.StartNew(() => blockchainEventListener.SybscribeForNewEraElectionComplitedEvent(GetGoldPriceOracleTokenAddress(config)));
            Task.Factory.StartNew(() => blockchainEventListener.SubscribeForEndEraByNewElectedChairman(GetGoldPriceOracleTokenAddress(config)));

            await CreateHostBuilder(null).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

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