using GoldPriceOracle.Services.Interfaces;
using GoldPriceOracle.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GoldPriceOracle.Node.RegistrationModules
{
    public static class ServicesRegistrationModule
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<ISetupService, SetupService>();
            services.AddScoped<IInformationService, InformationService>();
            services.AddScoped<IIntegrationService, IntegrationService>();
            services.AddScoped<IStakingManagerService, StakingManagerService>();
            services.AddScoped<IProofOfStakeService, ProofOfStakeService>();
        }
    }
}