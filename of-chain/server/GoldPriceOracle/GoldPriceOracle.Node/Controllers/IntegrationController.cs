using GoldPriceOracle.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GoldPriceOracle.Node.Controllers
{
    [Route("api")]
    public class IntegrationController : BaseController
    {
        private readonly IIntegrationService _integrationService;

        public IntegrationController(IIntegrationService integrationService)
        {
            _integrationService = integrationService;
        }

        [HttpGet("/price-data")]
        public async Task<IActionResult> GetPriceData([FromQuery] string assetCode, [FromQuery] string currencyCode)
            => HandleResponse(await _integrationService.GetAssetPriceModelAsync(assetCode, currencyCode));
    }
}