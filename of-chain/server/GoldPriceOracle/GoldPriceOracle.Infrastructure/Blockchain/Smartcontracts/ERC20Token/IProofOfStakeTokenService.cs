using GoldPriceOracle.Connection.Blockchain.ERC20Token;
using System.Numerics;
using System.Threading.Tasks;

namespace GoldPriceOracle.Infrastructure.Blockchain.Smartcontracts.ERC20Token
{
    public interface IProofOfStakeTokenService
    {
        Task<GetCurrentEraOutputDTO> GetCurrentEraAsync();

        Task EndCurrentEraAsync(BigInteger timestamp);

        Task<GetStakeholdersOutputDTO> GetStakeholdersAsync();

        Task VoteForProposedEraRequestAsync(VoteForProposedEraFunction voteForProposedEraFunction);

        Task ProposeNewEraElectionAsync(ProposeNewEraFunction proposeNewEraFunction);

        Task<GetEraByIdOutputDTO> GetEraByIdAsync(GetEraByIdFunction getEraByIdFunction);
    }
}