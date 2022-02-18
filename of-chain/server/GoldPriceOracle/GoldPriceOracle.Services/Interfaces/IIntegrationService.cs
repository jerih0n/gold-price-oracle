using GoldPriceOracle.Infrastructure.API.Response;
using GoldPriceOracle.Services.Models.Setup.Integration;
using System.Threading.Tasks;

namespace GoldPriceOracle.Services.Interfaces
{
    public interface IIntegrationService
    {
        Task<TryResult<AssetPriceModel>> GetAssetPriceModelAsync(string assetCode, string currencyCode);
    }
}
