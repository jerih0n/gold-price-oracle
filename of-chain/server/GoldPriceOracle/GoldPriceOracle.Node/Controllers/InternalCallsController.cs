using GoldPriceOracle.Node.Contracts.InternalCalls;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GoldPriceOracle.Node.Controllers
{
    [Route("internal")]
    public class InternalCallsController : BaseController
    {
        [HttpPost("new-price-round")]
        public async Task<IActionResult> NewRoundPriceStarted([FromBody] NewPriceRoundEventContract newRoundContract)
        {
            return Ok();
        }
    }
}
