using GoldPriceOracle.Configuration;
using GoldPriceOracle.Connection.Blockchain.ContractsServices.ERC20Token;
using GoldPriceOracle.Connection.Blockchain.ERC20Token;
using GoldPriceOracle.Connection.Database;
using Microsoft.Extensions.Options;
using System.Numerics;
using System.Threading.Tasks;

namespace GoldPriceOracle.Infrastructure.Blockchain.Smartcontracts.ERC20Token
{
    public class GoldPriceOracleERC20TokenService : BaseSmartContractCallerService, IGoldPriceOracleERC20TokenService
    {
        private readonly GoldOracleTokenService _goldOracleTokenService;
        private readonly GoldPriceOracleERC20TokenOptions _goldPriceOracleERC20TokenOptions;

        public GoldPriceOracleERC20TokenService(IOptionsMonitor<BlockchainNetworkOptions> blockchainNetworOptionskMonitor, 
            OracleDbContext oracleDbContext,
            IOptionsMonitor<GoldPriceOracleERC20TokenOptions> goldPriceOracleErc20TokenOptionsManager) 
            : base(blockchainNetworOptionskMonitor,
                  oracleDbContext)
        {
            _goldPriceOracleERC20TokenOptions = goldPriceOracleErc20TokenOptionsManager.CurrentValue;
            _goldOracleTokenService = new GoldOracleTokenService(Web3, _goldPriceOracleERC20TokenOptions.Address);
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

        public string TokenSymbol { get; }
    }
}
