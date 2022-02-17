

namespace GoldPriceOracle.Node.Contracts.Setup
{
    public class RestoreWithMnemonicContract : SetupNodeContract
    {
        public RestoreWithMnemonicContract(string mnemonic, string password) : base(password)
        {
            Mnemonic = mnemonic;
        }
        public string Mnemonic { get; }
    }
}
