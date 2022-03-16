using GoldPriceOracle.Infrastructure.API.Response;
using GoldPriceOracle.Services.Models.ProofOfStake;
using GoldPriceOracle.Services.Models.Voting;
using System.Threading.Tasks;

namespace GoldPriceOracle.Services.Interfaces
{
    public interface IProofOfStakeService
    {
        Task<TryResult<VotingResult>> TryProposeNewEraElectionAsync(string utcTimeStamp, string eraId);

        Task<TryResult<VotingResult>> TryVoteForNewEraElectionAsync(NewEraProposal newEraProposal);

        Task<TryResult<bool>> TryEndCurrentEraAsync(string eraId, string chairman);
    }
}