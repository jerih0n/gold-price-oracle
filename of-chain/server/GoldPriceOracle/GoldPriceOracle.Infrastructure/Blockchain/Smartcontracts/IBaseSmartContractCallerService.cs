using System.Numerics;
using System.Threading.Tasks;

namespace GoldPriceOracle.Infrastructure.Blockchain.Smartcontracts
{
    public interface IBaseSmartContractCallerService
    {
        Task<BigInteger> GetEthAmountAsync(string address);
    }
}
