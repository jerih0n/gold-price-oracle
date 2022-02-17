using NBitcoin;
using Nethereum.Web3.Accounts;

namespace GoldPriceOracle.Infrastructure.Blockchain.Accounts
{
    public class HDWalletManagingService : IHDWalletManagingService
    {
        public (Account, string) CreateNewWallet()
        {           
            Mnemonic mnemo = new Mnemonic(Wordlist.English, WordCount.Twelve);
            var privateKey = mnemo.DeriveExtKey().PrivateKey;

            return (new Account(privateKey.ToBytes()), mnemo.ToString());
            
        }

        public Account RestoreWalletFromMnemonic(string mnemonicPhrase)
        {           
            Mnemonic mnemo = new Mnemonic(mnemonicPhrase);
            var privateKey = mnemo.DeriveExtKey().PrivateKey;

            return new Account(privateKey.ToBytes());
            
        }
    }
}
