using GoldPriceOracle.Infrastructure.API.Response;
using GoldPriceOracle.Services.Models.Information;
using System.Threading.Tasks;

namespace GoldPriceOracle.Services.Interfaces
{
    public interface IInformationService
    {
        TryResult<MnemonicPhraseModel> GetMnemonicPhrase(string password);

        TryResult<AddressInformation> GetNodeActiveAddress(string password);

        Task<TryResult<OracleTokenBalance>> GetTokenBalanceAsync(string password);

        Task<TryResult<OracleTokenBalance>> GetStakedAmountAsync(string password);

        Task<TryResult<StakeholderInformationModel>> GetStakeholderInformationAsync(string password);

        Task<TryResult<OracleTokenBalance>> GetEthereumBalanceAsync(string password);
    }
}