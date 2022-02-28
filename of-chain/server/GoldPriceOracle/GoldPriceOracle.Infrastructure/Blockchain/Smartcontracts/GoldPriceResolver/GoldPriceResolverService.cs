using GoldPriceOracle.Configuration;
using GoldPriceOracle.Connection.Database;
using Microsoft.Extensions.Options;


namespace GoldPriceOracle.Infrastructure.Blockchain.Smartcontracts.GoldPriceResolver
{
    public class GoldPriceResolverService : BaseSmartContractCallerService, IGoldPriceResolverService
    {
       public GoldPriceResolverService(IOptionsMonitor<GoldPriceResolverOptions> goldPriceResolverOptionsMonitor,
           IOptionsMonitor<BlockchainNetworkOptions> blockchainNetworkOptionsMonitor, 
           OracleDbContext oracleDbContext) : 
            base(blockchainNetworkOptionsMonitor, oracleDbContext)
       {

       }
    }
}
