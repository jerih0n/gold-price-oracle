using GoldPriceOracle.Node.Contracts.Setup;
using GoldPriceOracle.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [HttpPost("token-balance")]
        public async Task<IActionResult> GetOracleTokenBalanceAsync([FromBody] PasswordContract passwordContract)
            => HandleResponse(await _informationService.GetTokenBalanceAsync(passwordContract.Password));

        [HttpPost("staked-amount")]
        public async Task<IActionResult> GetOracleStakedAmountAsync([FromBody] PasswordContract passwordContract)
            => HandleResponse(await _informationService.GetStakedAmountAsync(passwordContract.Password));

        [HttpPost("stakeholder")]
        public async Task<IActionResult> GetStakeholderInfomationAsync([FromBody] PasswordContract passwordContract)
            => HandleResponse(await _informationService.GetStakeholderInformationAsync(passwordContract.Password));
    }
}
