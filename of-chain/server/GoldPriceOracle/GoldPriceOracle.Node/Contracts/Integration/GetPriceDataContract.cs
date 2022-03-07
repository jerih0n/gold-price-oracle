using GoldPriceOracle.Node.Contracts.Setup;
using System.ComponentModel.DataAnnotations;

namespace GoldPriceOracle.Node.Contracts.Integration
{
    public class GetPriceDataContract
    {
        [Required(ErrorMessage = "Asset code is required")]
        public string AssetCode { get; set; }

        [Required(ErrorMessage = "Asset code is required")]
        public string FiatCurrencyCode { get; set; }
    }
}