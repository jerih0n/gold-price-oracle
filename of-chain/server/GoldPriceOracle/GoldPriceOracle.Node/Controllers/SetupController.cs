using Microsoft.AspNetCore.Mvc;

namespace GoldPriceOracle.Node.Controllers
{
    [Route("api/setup")]
    public class SetupController : ControllerBase
    {
        public SetupController()
        {

        }

        [HttpGet("check")]
        public IActionResult CheckNodeSetup()
        {
            return Ok();
        }

    }
}
