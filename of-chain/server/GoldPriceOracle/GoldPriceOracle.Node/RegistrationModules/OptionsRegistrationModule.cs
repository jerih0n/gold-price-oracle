using GoldPriceOracle.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoldPriceOracle.Node.RegistrationModules
{
    public static class OptionsRegistrationModule
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GoldAPIServiceOptions>(configuration.GetSection("GoldAPIService"));
            services.Configure<BlockchainNetworkOptions>(configuration.GetSection("Blockchain:BlockchainNetwork"));
            services.Configure<GoldPriceOracleERC20TokenOptions>(configuration.GetSection("Blockchain:SmartContracts:GoldPriceOracleERC20Token"));

        }
    }
}
