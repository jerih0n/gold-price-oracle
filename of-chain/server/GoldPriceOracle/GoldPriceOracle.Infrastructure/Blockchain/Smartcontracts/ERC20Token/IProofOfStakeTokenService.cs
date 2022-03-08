using GoldPriceOracle.Connection.Blockchain.ERC20Token;
using System.Threading.Tasks;

namespace GoldPriceOracle.Infrastructure.Blockchain.Smartcontracts.ERC20Token
{
    public interface IProofOfStakeTokenService
    {
        Task<GetCurrentEraOutputDTO> GetCurrentEraAsync();

        Task EndCurrentEraAsync();

        Task StartNewEraAsync();

        Task<GetStakeholdersOutputDTO> GetStakeholdersAsync();
    }
}