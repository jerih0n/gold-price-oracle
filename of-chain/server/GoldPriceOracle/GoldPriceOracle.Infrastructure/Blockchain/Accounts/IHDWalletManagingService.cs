using Nethereum.Web3.Accounts;

namespace GoldPriceOracle.Infrastructure.Blockchain.Accounts
{
    public interface IHDWalletManagingService
    {
        Account CreateNewWallet();

        Account RestoreWalletFromMnemonic(string mnemonicPhrase);
    }
}
