using GoldPriceOracle.Node.Contracts.Setup;
using GoldPriceOracle.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoldPriceOracle.Node.Controllers
{
    [Route("api/info")]
    public class NodeInformationController : BaseController
    {
        private readonly IInformationService _informationService;

        public NodeInformationController(IInformationService informationService)
        {
            _informationService = informationService;
        }

        [HttpPost("mnemonic")]
        public IActionResult GetMnemonicPhrase([FromBody] PasswordContract passwordContract)
            => HandleResponse(_informationService.GetMnemonicPhrase(passwordContract.Password));

        [HttpPost("active-address")]
        public IActionResult GetActiveAddress([FromBody] PasswordContract passwordContract)
            => HandleResponse(_informationService.GetNodeActiveAddress(passwordContract.Password));
    }
}
