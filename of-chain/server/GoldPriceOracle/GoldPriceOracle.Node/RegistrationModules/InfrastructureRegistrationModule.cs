﻿using GoldPriceOracle.Infrastructure.Blockchain.Accounts;
using GoldPriceOracle.Infrastructure.Blockchain.Smartcontracts.ERC20Token;
using GoldPriceOracle.Infrastructure.DatabaseAccessServices;
using GoldPriceOracle.Infrastructure.Integration.ExternalAPI;
using Microsoft.Extensions.DependencyInjection;

namespace GoldPriceOracle.Node.RegistrationModules
{
    public static class InfrastructureRegistrationModule
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IHDWalletManagingService, HDWalletManagingService>();
            services.AddScoped<INodeDataDataAccessService, NodeDataAccessService>();
            services.AddScoped<IExternalGoldApiIntegrationService, ExternalGoldApiIntegrationService>();
            services.AddScoped<IAssetPriceHistoricalDataAccessService, AssetPriceHistoricalDataAccessService>();
            services.AddScoped<IGoldPriceOracleERC20TokenService, GoldPriceOracleERC20TokenService>();
        }
    }
}