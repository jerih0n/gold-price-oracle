using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace GoldPriceOracle.Infrastructure.Integration.ExternalAPI.Models
{
    public class ExterlApiCallResponse
    {
        [JsonProperty("timestamp")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime TimeStamp { get; set; }

        [JsonProperty("metal")]
        public string MetalCode { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
