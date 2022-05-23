using GoldPriceOracle.Configuration;
using GoldPriceOracle.Connection.Blockchain.ContractsServices.GoldPriceResolver;
using GoldPriceOracle.Connection.Database;
using Microsoft.Extensions.Options;
using System.Numerics;
using System.Threading.Tasks;

namespace GoldPriceOracle.Infrastructure.Blockchain.Smartcontracts.GoldPriceResolver
{
    public class GoldPriceResolverSmartcontractService : BaseSmartContractCallerService, IGoldPriceResolverSmartcontractService
    {
        private readonly GoldPriceResolverService _goldPriceResolverService;

        public GoldPriceResolverSmartcontractService(IOptionsMonitor<GoldPriceResolverOptions> goldPriceResolverOptionsMonitor,
           IOptionsMonitor<BlockchainNetworkOptions> blockchainNetworkOptionsMonitor,
           OracleDbContext oracleDbContext) :
            base(goldPriceResolverOptionsMonitor,
                blockchainNetworkOptionsMonitor,
                oracleDbContext)
        {
            _goldPriceResolverService = new GoldPriceResolverService(Web3, Address);
        }

        public async Task TryVoteForPriceRoundAsync(byte[] roundId, BigInteger price)
            => await _goldPriceResolverService.VotePriceForRoundRequestAsync(roundId, price);
    }
}