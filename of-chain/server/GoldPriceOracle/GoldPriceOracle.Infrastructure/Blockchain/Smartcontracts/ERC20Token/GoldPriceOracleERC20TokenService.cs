using GoldPriceOracle.Configuration;
using GoldPriceOracle.Connection.Blockchain.ContractsServices.ERC20Token;
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
            IOptionsMonitor<GoldPriceOracleERC20TokenOptions> goldPriceOracleErc20TokenOptionsManager) 
            : base(blockchainNetworOptionskMonitor)
        {
            _goldPriceOracleERC20TokenOptions = goldPriceOracleErc20TokenOptionsManager.CurrentValue;
            _goldOracleTokenService = new GoldOracleTokenService(Web3, _goldPriceOracleERC20TokenOptions.Address);
            TokenSymbol = "GOT";
        }

        public async Task<BigInteger> GetBalanceAsync(string address)
            => await _goldOracleTokenService.BalanceOfQueryAsync(address);

        public async Task<BigInteger> GetStakedBalanceAsync(string address)
            => await _goldOracleTokenService.GetStakedAmountQueryAsync(address);

        public string TokenSymbol { get; }
    }
}
