using System.ComponentModel.DataAnnotations;

namespace GoldPriceOracle.Node.Contracts.Setup
{
    public class RestoreWithMnemonicContract : PasswordContract
    {
        [Required]
        public string Mnemonic { get; set; }
    }
}