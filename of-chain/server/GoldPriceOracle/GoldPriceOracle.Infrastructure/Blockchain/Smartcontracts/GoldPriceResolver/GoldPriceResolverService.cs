using GoldPriceOracle.Configuration;
using GoldPriceOracle.Connection.Blockchain.Contracts.GoldPriceResolver;
using GoldPriceOracle.Connection.Database;
using Microsoft.Extensions.Options;
using Nethereum.Contracts;
using Nethereum.JsonRpc.WebSocketStreamingClient;
using Nethereum.RPC.Reactive.Eth.Subscriptions;
using System;
using System.Threading.Tasks;

namespace GoldPriceOracle.Infrastructure.Blockchain.Smartcontracts.GoldPriceResolver
{
    public class GoldPriceResolverService : BaseSmartContractCallerService, IGoldPriceResolverService
    {
       public GoldPriceResolverService(IOptionsMonitor<GoldPriceResolverOptions> goldPriceResolverOptionsMonitor,
           IOptionsMonitor<BlockchainNetworkOptions> blockchainNetworkOptionsMonitor, 
           OracleDbContext oracleDbContext) : 
            base(goldPriceResolverOptionsMonitor,
                blockchainNetworkOptionsMonitor, 
                oracleDbContext)
       {

       }
    }
}
