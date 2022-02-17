using NBitcoin;
using Nethereum.Web3.Accounts;
using System;

namespace GoldPriceOracle.Infrastructure.Blockchain.Accounts
{
    public class HDWalletManagingService : IHDWalletManagingService
    {
        public Account CreateNewWallet()
        {
            try
            {
                Mnemonic mnemo = new Mnemonic(Wordlist.English, WordCount.Twelve);
                var privateKey = mnemo.DeriveExtKey().PrivateKey;
                return new Account(privateKey.ToBytes());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public Account RestoreWalletFromMnemonic(string mnemonicPhrase)
        {
            try
            {
                Mnemonic mnemo = new Mnemonic(mnemonicPhrase);
                var privateKey = mnemo.DeriveExtKey().PrivateKey;

                return new Account(privateKey.ToBytes());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
