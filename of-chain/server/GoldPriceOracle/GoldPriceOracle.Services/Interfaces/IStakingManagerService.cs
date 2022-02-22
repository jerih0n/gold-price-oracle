using GoldPriceOracle.Infrastructure.API.Response;
using System.Threading.Tasks;

namespace GoldPriceOracle.Services.Interfaces
{
    public interface IStakingManagerService
    {
        Task<TryResult<bool>> StakeAmountAsync(string password, decimal amount);
    }
}
