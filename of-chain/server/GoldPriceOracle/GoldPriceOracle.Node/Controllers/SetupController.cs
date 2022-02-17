using GoldPriceOracle.Node.Contracts.Setup;
using GoldPriceOracle.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoldPriceOracle.Node.Controllers
{
    [Route("api/setup")]
    public class SetupController : ControllerBase
    {
        private readonly ISetupService _setupService;

        public SetupController(ISetupService setupService)
        {
            _setupService = setupService;
        }

        [HttpGet("check")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult CheckNodeSetup()
        {
            var result = _setupService.IsNodeSetup();

            return result.IsSuccessfull ? Ok(result.Item) : StatusCode((int)result.Error.Code, result.Error);
        }

        [HttpPost("new")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public IActionResult SetUpNode([FromBody] SetupNodeContract setupNodeContract)
        {
            var result = _setupService.SetupNode(setupNodeContract.Password);

            return result.IsSuccessfull ? Ok(result.Item) : StatusCode((int)result.Error.Code, result.Error);
        }

        [HttpPost("seed-restore")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public IActionResult RestoreNode([FromBody]RestoreWithMnemonicContract restoreWithMnemonicContract)
        {
            var result = _setupService.RestoreFromMnemonic(restoreWithMnemonicContract.Mnemonic, restoreWithMnemonicContract.Password);

            return result.IsSuccessfull ? Ok(result.Item) : StatusCode((int)result.Error.Code, result.Error);
        }
    }
}
