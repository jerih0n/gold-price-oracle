using GoldPriceOracle.Infrastructure.API.Response;
using GoldPriceOracle.Services.Models.Information;

namespace GoldPriceOracle.Services.Interfaces
{
    public interface IInformationService
    {
        TryResult<MnemonicPhraseModel> GetMnemonicPhrase(string password);
    }
}
