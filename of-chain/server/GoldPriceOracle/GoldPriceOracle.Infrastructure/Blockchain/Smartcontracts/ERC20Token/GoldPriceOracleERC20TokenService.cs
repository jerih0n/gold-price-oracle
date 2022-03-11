using GoldPriceOracle.Configuration;
using GoldPriceOracle.Connection.Blockchain.ContractsServices.ERC20Token;
using GoldPriceOracle.Connection.Blockchain.ERC20Token;
using GoldPriceOracle.Connection.Database;
using Microsoft.Extensions.Options;
using System.Numerics;
using System.Threading.Tasks;

namespace GoldPriceOracle.Infrastructure.Blockchain.Smartcontracts.ERC20Token
{
    public class GoldPriceOracleERC20TokenService : BaseSmartContractCallerService, IGoldPriceOracleERC20TokenService, IProofOfStakeTokenService
    {
        private readonly GoldOracleTokenService _goldOracleTokenService;

        public GoldPriceOracleERC20TokenService(IOptionsMonitor<GoldPriceOracleERC20TokenOptions> goldPriceOracleErc20TokenOptionsManager,
            IOptionsMonitor<BlockchainNetworkOptions> blockchainNetworOptionskMonitor,
            OracleDbContext oracleDbContext)
            : base(goldPriceOracleErc20TokenOptionsManager,
                  blockchainNetworOptionskMonitor,
                  oracleDbContext)
        {
            _goldOracleTokenService = new GoldOracleTokenService(Web3, Address);
            TokenSymbol = "GOT";
        }

        public async Task<BigInteger> GetBalanceAsync(string address)
            => await _goldOracleTokenService.BalanceOfQueryAsync(address);

        public async Task<BigInteger> GetStakedBalanceAsync(string address)
            => await _goldOracleTokenService.GetStakedAmountQueryAsync(address);

        public async Task StakeAmountAsync(BigInteger amount)
           => await _goldOracleTokenService.StakeRequestAsync(amount);

        public async Task UnstakeAmountAsync(BigInteger amount)
            => await _goldOracleTokenService.UnstakeRequestAsync(amount);

        public async Task<GetStakeholderInformationOutputDTO> GetStakeholderInfomationAsync(string address)
            => await _goldOracleTokenService.GetStakeholderInformationQueryAsync(address);

        public async Task<GetStakeholdersOutputDTO> GetStakeholdersAsync()
            => await _goldOracleTokenService.GetStakeholdersQueryAsync();

        public async Task<GetCurrentEraOutputDTO> GetCurrentEraAsync()
            => await _goldOracleTokenService.GetCurrentEraQueryAsync();

        public async Task EndCurrentEraAsync(BigInteger timestamp)
            => await _goldOracleTokenService.EndEraRequestAndWaitForReceiptAsync(timestamp);

        public async Task VoteForProposedEraRequestAsync(VoteForProposedEraFunction voteForProposedEraFunction)
            => await _goldOracleTokenService.VoteForProposedEraRequestAsync(voteForProposedEraFunction);

        public async Task ProposeNewEraElectionAsync(ProposeNewEraFunction proposeNewEraFunction)
            => await _goldOracleTokenService.ProposeNewEraRequestAndWaitForReceiptAsync(proposeNewEraFunction);

        public string TokenSymbol { get; }
    }
}