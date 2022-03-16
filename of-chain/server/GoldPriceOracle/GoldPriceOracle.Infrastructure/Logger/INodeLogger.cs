namespace GoldPriceOracle.Infrastructure.Integration.Logger
{
    public interface INodeLogger
    {
        void LogInformation(string message);

        void LogInformation(string message, object logData);
    }
}