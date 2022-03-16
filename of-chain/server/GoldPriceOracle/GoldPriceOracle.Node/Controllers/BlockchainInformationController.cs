using GoldPriceOracle.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GoldPriceOracle.Node.Controllers
{
    [Route("blockchain-information")]
    public class BlockchainInformationController : BaseController
    {
        private readonly IBlockchainMonitorService _blockchainMonitorService;

        public BlockchainInformationController(IBlockchainMonitorService blockchainMonitorService)
        {
            _blockchainMonitorService = blockchainMonitorService;
        }

        [HttpGet("era-by-id/{eraId}")]
        public async Task<IActionResult> GetEraByIdAsync([FromRoute] string eraId)
            => HandleResponse(await _blockchainMonitorService.GetEraByIdAsync(eraId));

        [HttpGet("current-era")]
        public async Task<IActionResult> GetEraByIdAsync()
            => HandleResponse(await _blockchainMonitorService.GetCurrentEraAsync());
    }
}