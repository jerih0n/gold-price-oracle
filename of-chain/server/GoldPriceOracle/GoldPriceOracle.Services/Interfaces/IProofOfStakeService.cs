using GoldPriceOracle.Infrastructure.API.Response;
using GoldPriceOracle.Services.Models.Voting;
using System.Threading.Tasks;

namespace GoldPriceOracle.Services.Interfaces
{
    public interface IProofOfStakeService
    {
        Task<TryResult<VotingResult>> TryProposeNewEraElectionAsync(string utcTimeStamp, string eraId);
    }
}