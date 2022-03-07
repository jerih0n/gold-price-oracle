using System.ComponentModel.DataAnnotations;

namespace GoldPriceOracle.Node.Contracts.Setup
{
    public class PasswordContract
    {
        [Required]
        public string Password { get; set; }
    }
}