using GoldPriceOracle.Infrastructure.Utils;
using GoldPriceOracle.Node.Contracts.InternalCalls;
using GoldPriceOracle.Services.Interfaces;
using GoldPriceOracle.Services.Models.ProofOfStake;
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

        [HttpPost("era-proposal-vote")]
        public async Task<IActionResult> TryVoteForEraElectionProposalAsync([FromBody] NewEraProposalEventContract newEraProposalEventContract)
        {
            var newEraProposal = new NewEraProposal(newEraProposalEventContract.EraId,
                newEraProposalEventContract.Chairman,
                newEraProposalEventContract.Council,
                newEraProposalEventContract.ValidatorsCount,
                newEraProposalEventContract.CalculatedSeed);

            return HandleResponse(await _proofOfStakeService.TryVoteForNewEraElectionAsync(newEraProposal));
        }

        [HttpPost("new-era-election-complited")]
        public async Task<IActionResult> NewEraElectionComplitedNotificationAsync([FromBody] NewEraElectionComplitedContract newEraElectionComplitedContract)
            => HandleResponse(await _proofOfStakeService.TryEndCurrentEraAsync(newEraElectionComplitedContract.Era.Id,
                newEraElectionComplitedContract.Era.Chairman));

        [HttpPost("end-era")]
        public async Task<IActionResult> EndEraByNewElectedChairmanContractAsync([FromBody] EndEraByNewElectedChairmanContract endEraByNewElectedChairmanContract)
            => HandleResponse(await _proofOfStakeService.RecordEndedEraAsync(endEraByNewElectedChairmanContract.EraId,
                endEraByNewElectedChairmanContract.Chairman,
                endEraByNewElectedChairmanContract.Timestap));
    }
}