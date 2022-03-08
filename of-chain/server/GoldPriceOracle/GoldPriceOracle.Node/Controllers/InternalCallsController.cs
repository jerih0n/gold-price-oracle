using GoldPriceOracle.Infrastructure.Utils;
using GoldPriceOracle.Node.Contracts.InternalCalls;
using GoldPriceOracle.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GoldPriceOracle.Node.Controllers
{
    [Route("internal")]
    public class InternalCallsController : BaseController
    {
        private readonly IProofOfStakeService _proofOfStakeService;

        public InternalCallsController(IProofOfStakeService proofOfStakeService)
        {
            _proofOfStakeService = proofOfStakeService;
        }

        [HttpPost("price-round-vote")]
        public async Task<IActionResult> NewRoundPriceVoteAsync([FromBody] NewPriceRoundEventContract newRoundContract)
        {
            var rouidId = newRoundContract.RoundId.ToByteArray();

            return Ok();
        }

        [HttpPost("start-new-price-round")]
        public async Task<IActionResult> NewPriceRoundAsync([FromBody] StartNewPriceRoundContract starNewEraEventContract)
        {
            return Ok();
        }

        [HttpPost("new-era-election")]
        public async Task<IActionResult> TyrProposeNewElectionEraAsync([FromBody] StarNewEraEventContract starNewEraEventContract)
            => HandleResponse(await _proofOfStakeService.TryProposeNewEraElectionAsync(starNewEraEventContract.UtcTimeStamp, starNewEraEventContract.EraId));
    }
}