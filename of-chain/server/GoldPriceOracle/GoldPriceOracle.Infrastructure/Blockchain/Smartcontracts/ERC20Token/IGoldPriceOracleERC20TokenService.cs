using GoldPriceOracle.Connection.Blockchain.ERC20Token;
using System.Numerics;
using System.Threading.Tasks;

namespace GoldPriceOracle.Infrastructure.Blockchain.Smartcontracts.ERC20Token
{
    public interface IGoldPriceOracleERC20TokenService : IBaseSmartContractCallerService
    {
        Task<BigInteger> GetBalanceAsync(string address);

        public string TokenSymbol { get; }

        Task<BigInteger> GetStakedBalanceAsync(string address);

        Task StakeAmountAsync(BigInteger amount);

        Task UnstakeAmountAsync(BigInteger amount);

        Task<GetStakeholderInformationOutputDTO> GetStakeholderInfomationAsync(string address);

        Task<GetStakeholdersOutputDTO> GetStakeholdersAsync();
    }
}