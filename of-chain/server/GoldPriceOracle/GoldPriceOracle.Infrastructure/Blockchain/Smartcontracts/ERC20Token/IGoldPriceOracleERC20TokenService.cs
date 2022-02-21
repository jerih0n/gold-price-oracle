using System.Numerics;
using System.Threading.Tasks;

namespace GoldPriceOracle.Infrastructure.Blockchain.Smartcontracts.ERC20Token
{
    public interface IGoldPriceOracleERC20TokenService
    {
        Task<BigInteger> GetBalanceAsync(string address);

        public string TokenSymbol { get; }

        Task<BigInteger> GetStakedBalanceAsync(string address);
    }
}
