using System;

namespace GoldPriceOracle.Services.Models.Setup.Integration
{
    public class AssetPriceModel
    {
        public AssetPriceModel(DateTime timeStamp, 
            string metalCode,
            string currencyCode, 
            decimal price)
        {
            Timestamp = timeStamp;
            MetalCode = metalCode;
            CurrencyCode = currencyCode;
            Price = price;
        }

        public DateTime Timestamp { get; }

        public string MetalCode { get;  }

        public string CurrencyCode { get;}

        public decimal Price { get;  }
    }
}
