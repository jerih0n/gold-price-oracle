namespace GoldPriceOracle.Services.Interfaces
{
    public interface ISetupService
    {
        bool IsNodeSetup();
        bool SetupNode(string password);
    }
}
