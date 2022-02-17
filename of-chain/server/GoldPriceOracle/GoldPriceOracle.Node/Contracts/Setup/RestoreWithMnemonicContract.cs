using System.ComponentModel.DataAnnotations;

namespace GoldPriceOracle.Node.Contracts.Setup
{
    public class RestoreWithMnemonicContract : SetupNodeContract
    {
        [Required]
        public string Mnemonic { get; set; }
    }
}
