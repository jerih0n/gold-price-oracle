using GoldPriceOracle.Infrastructure.API.Response;
using GoldPriceOracle.Services.Models.ProofOfStake;
using System.Threading.Tasks;

namespace GoldPriceOracle.Services.Interfaces
{
    public interface IBlockchainMonitorService
    {
        Task<TryResult<EraInformation>> GetEraByIdAsync(string eraId);

        Task<TryResult<EraInformation>> GetCurrentEraAsync();
    }
}