using GoldPriceOracle.Infrastructure.API.Response;
using GoldPriceOracle.Infrastructure.Integration.ExternalAPI;
using GoldPriceOracle.Services.Interfaces;
using GoldPriceOracle.Services.Models.Setup.Integration;
using System;
using System.Net;
using System.Threading.Tasks;

namespace GoldPriceOracle.Services.Services
{
    public class IntegrationService : IIntegrationService
    {
        private readonly IExternalGoldApiIntegrationService _externalGoldApiIntegrationService;

        public IntegrationService(IExternalGoldApiIntegrationService externalGoldApiIntegrationService)
        {
            _externalGoldApiIntegrationService = externalGoldApiIntegrationService;
        }

        public async Task<TryResult<AssetPriceModel>> GetAssetPriceModelAsync(string assetCode, string currencyCode)
        {
            try
            {
                var result = await _externalGoldApiIntegrationService.GetAssetPriceAsync(assetCode, currencyCode);

                if (!result.IsSuccessfull) return TryResult<AssetPriceModel>.Fail(result.Error);

                var assetPriceModel = new AssetPriceModel(result.Item.TimeStamp,
                    result.Item.MetalCode,
                    result.Item.Currency,
                    result.Item.Price);

                return TryResult<AssetPriceModel>.Success(assetPriceModel);
            }
            catch (Exception ex)
            {
                return TryResult<AssetPriceModel>.Fail(new ApiError(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}