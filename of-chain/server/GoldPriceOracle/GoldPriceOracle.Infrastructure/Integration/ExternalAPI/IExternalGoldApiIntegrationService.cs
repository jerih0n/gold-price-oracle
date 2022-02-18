using GoldPriceOracle.Infrastructure.API.Response;
using GoldPriceOracle.Infrastructure.Integration.ExternalAPI.Models;
using System.Threading.Tasks;

namespace GoldPriceOracle.Infrastructure.Integration.ExternalAPI
{
    public interface IExternalGoldApiIntegrationService
    {
        Task<TryResult<ExterlApiCallResponse>> GetAssetPriceAsync(string assetCode, string currencyCode);
    }
}
