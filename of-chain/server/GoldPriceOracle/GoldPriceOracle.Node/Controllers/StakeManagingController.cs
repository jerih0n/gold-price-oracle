using GoldPriceOracle.Node.Contracts.StakeManager;
using GoldPriceOracle.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GoldPriceOracle.Node.Controllers
{
    [Route("/api/stake-manager")]
    public class StakeManagingController : BaseController
    {
        private readonly IStakingManagerService _stakingManagerService;

        public StakeManagingController(IStakingManagerService stakingManagerService)
        {
            _stakingManagerService = stakingManagerService;
        }

        [HttpPost("/stake")]
        public async Task<IActionResult> StakeAmountAsync([FromBody] StakeOrUnstakeContract stakeOrUnstakeContract)
            => HandleResponse(await _stakingManagerService.StakeAmountAsync(stakeOrUnstakeContract.Password, stakeOrUnstakeContract.Amount));

        [HttpPost("/unstake")]
        public async Task<IActionResult> UnstakeAmountAsync([FromBody] StakeOrUnstakeContract stakeOrUnstakeContract)
            => HandleResponse(await _stakingManagerService.UnstakeAmountAsync(stakeOrUnstakeContract.Password, stakeOrUnstakeContract.Amount));
    }
}