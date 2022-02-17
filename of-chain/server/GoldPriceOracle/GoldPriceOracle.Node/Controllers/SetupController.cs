﻿using GoldPriceOracle.Node.Contracts.Setup;
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
        public IActionResult CheckNodeSetup()
        {
            var result = _setupService.IsNodeSetup();
            return Ok(new BooleanResponse(result));
        }

        [HttpPost("new")]
        public IActionResult SetUpNode([FromBody]SetupNodeContract setupNodeContract)
        {
            var result = _setupService.SetupNode(setupNodeContract.Password);

            return Ok(new BooleanResponse(result));
        }

        [HttpPost["restore/seed"]]
        public IActionResult RestoreNode()
        {

        }
    }
}
