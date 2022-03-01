using GoldPriceOracle.Infrastructure.API.Response;
using GoldPriceOracle.Services.Models.PriceRound;
using System.Threading.Tasks;

namespace GoldPriceOracle.Services.Interfaces
{
    public interface IPriceRoundService
    {
        public Task<TryResult<PriceRoundVotingResult>> TryVoteForNewPriceRoundAsync(string roundId,
            string priceAsBigNumber,
            string proposalAddress,
            string assetCode,
            string currencyCode);
    }
}
