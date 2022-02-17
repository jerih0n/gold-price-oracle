using Nethereum.Web3.Accounts;

namespace GoldPriceOracle.Infrastructure.Blockchain.Accounts
{
    public interface IHDWalletManagingService
    {
        (Account, string) CreateNewWallet();

        Account RestoreWalletFromMnemonic(string mnemonicPhrase);
    }
}
