using System;

namespace GoldPriceOracle.Services.Models.Setup.Integration
{
    public class AssetPriceModel
    {
        public AssetPriceModel(DateTimeOffset timestamp, 
            string metalCode,
            string currencyCode, 
            decimal price)
        {
            Timestamp = timestamp;
            MetalCode = metalCode;
            CurrencyCode = currencyCode;
            Price = price;
        }

        public DateTimeOffset Timestamp { get; }

        public string MetalCode { get;  }

        public string CurrencyCode { get;}

        public decimal Price { get;  }
    }
}
