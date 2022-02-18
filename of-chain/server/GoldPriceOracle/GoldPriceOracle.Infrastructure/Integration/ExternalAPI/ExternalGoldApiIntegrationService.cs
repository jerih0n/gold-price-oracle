using GoldPriceOracle.Configuration;
using GoldPriceOracle.Infrastructure.API.Response;
using GoldPriceOracle.Infrastructure.Integration.ExternalAPI.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoldPriceOracle.Infrastructure.Integration.ExternalAPI
{
    public class ExternalGoldApiIntegrationService : IExternalGoldApiIntegrationService
    {
        private readonly GoldAPIServiceOptions _options;

        public ExternalGoldApiIntegrationService(IOptionsMonitor<GoldAPIServiceOptions> options)
        {
            _options = options.CurrentValue;
        }

        public async Task<TryResult<ExterlApiCallResponse>> GetAssetPriceAsync(string assetCode, string currencyCode)
        {
            HttpClient httpClient = new HttpClient();

            var fullUrl = $"{_options.BaseUrl}/{assetCode}/{currencyCode}";

            var httpMessage = new HttpRequestMessage(HttpMethod.Get, fullUrl);
            httpMessage.Headers.Add("x-access-token", _options.ApiKey);

            var result =  await httpClient.SendAsync(httpMessage);

            if(!result.IsSuccessStatusCode)
            {
                return TryResult<ExterlApiCallResponse>.Fail(new ApiError(result.StatusCode, result.ReasonPhrase));
            }

            var content = await result.Content.ReadAsStringAsync();

            var goldPriceApiResponse = JsonConvert.DeserializeObject<ExterlApiCallResponse>(content);

            return TryResult<ExterlApiCallResponse>.Success(goldPriceApiResponse);
        }
    }
}
