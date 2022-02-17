using GoldPriceOracle.Infrastructure.API.Response;
using GoldPriceOracle.Services.Models.Setup;

namespace GoldPriceOracle.Services.Interfaces
{
    public interface ISetupService
    {
        TryResult<IsNodeSetUpModel> IsNodeSetup();

        TryResult<IsNodeSetUpModel> SetupNode(string password);

        TryResult<IsNodeSetUpModel> RestoreFromMnemonic(string mnemonic, string password);
    }
}
