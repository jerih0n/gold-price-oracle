

using System.Net;

namespace GoldPriceOracle.Infrastructure.API.Response
{
    public class ApiError
    {
        public ApiError(HttpStatusCode code, string description)
        {
            Code = code;
            Description = description;
        }
        public HttpStatusCode Code { get; }
        public string Description { get; }
    }
}
