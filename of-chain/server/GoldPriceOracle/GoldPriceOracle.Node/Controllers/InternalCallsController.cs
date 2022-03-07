using GoldPriceOracle.Infrastructure.Utils;
using GoldPriceOracle.Node.Contracts.InternalCalls;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GoldPriceOracle.Node.Controllers
{
    [Route("internal")]
    public class InternalCallsController : BaseController
    {
        [HttpPost("price-round-vote")]
        public async Task<IActionResult> NewRoundPriceVoteAsync([FromBody] NewPriceRoundEventContract newRoundContract)
        {
            var rouidId = newRoundContract.RoundId.ToByteArray();

            return Ok();
        }

        [HttpPost("start-new-price-round")]
        public async Task<IActionResult> NewPriceRoundAsync()
        {
            return Ok();
        }

        [HttpPost("new-era-start")]
        public async Task<IActionResult> StartNewEraAsync()
        {
            return Ok();
        }
    }
}