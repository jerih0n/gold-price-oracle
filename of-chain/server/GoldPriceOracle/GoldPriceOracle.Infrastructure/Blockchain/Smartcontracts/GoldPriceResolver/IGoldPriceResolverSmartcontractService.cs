using System.Numerics;
using System.Threading.Tasks;

namespace GoldPriceOracle.Infrastructure.Blockchain.Smartcontracts.GoldPriceResolver
{
    public interface IGoldPriceResolverSmartcontractService
    {
        Task TryVoteForPriceRoundAsync(byte[] roundId, BigInteger price);
    }
}
