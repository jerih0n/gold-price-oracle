using System.ComponentModel.DataAnnotations;

namespace GoldPriceOracle.Node.Contracts.Setup
{
    public class SetupNodeContract
    {
        [Required]
        public string Password { get; set; }
    }
}
