using GoldPriceOracle.Infrastructure.API.Response;
using Microsoft.AspNetCore.Mvc;

namespace GoldPriceOracle.Node.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IActionResult HandleResponse<TResponse>(TryResult<TResponse> response)
        {
            return response.IsSuccessfull ? Ok(response.Item) : StatusCode((int)response.Error.Code, response.Error);
        }
    }
}