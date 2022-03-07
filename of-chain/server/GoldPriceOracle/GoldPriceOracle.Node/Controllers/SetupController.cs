using GoldPriceOracle.Node.Contracts.Setup;
using GoldPriceOracle.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoldPriceOracle.Node.Controllers
{
    [Route("api/setup")]
    public class SetupController : BaseController
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
            => HandleResponse(_setupService.IsNodeSetup());

        [HttpPost("new")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public IActionResult SetUpNode([FromBody] PasswordContract setupNodeContract)
            => HandleResponse(_setupService.SetupNode(setupNodeContract.Password));

        [HttpPost("seed-restore")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public IActionResult RestoreNode([FromBody] RestoreWithMnemonicContract restoreWithMnemonicContract)
            => HandleResponse(_setupService.RestoreFromMnemonic(restoreWithMnemonicContract.Mnemonic, restoreWithMnemonicContract.Password));
    }
}