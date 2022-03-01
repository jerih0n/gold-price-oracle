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
            var blockchainEventListener = (BlockchainEventListener)Activator.CreateInstance(typeof(BlockchainEventListener), blockchainData);

            Task.Factory.StartNew(() => blockchainEventListener.SubscriteForNewPriceRoundEvent(GetGoldPriceResolverAddress(config)));

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
        
    }
}
