using GoldPriceOracle.Node.Contracts.Setup;
using System.ComponentModel.DataAnnotations;

namespace GoldPriceOracle.Node.Contracts.StakeManager
{
    public class StakeOrUnstakeContract : PasswordContract
    {
        [Required]
        public decimal Amount { get; set; }
    }
}
