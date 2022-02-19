using System.Numerics;
using System.Threading.Tasks;

namespace GoldPriceOracle.Infrastructure.Blockchain.Smartcontracts.ERC20Token
{
    public interface IGoldPriceOracleERC20TokenService
    {
        Task<BigInteger> GetBalance(string address);

        public string TokenSymbol { get; }
    }
}
