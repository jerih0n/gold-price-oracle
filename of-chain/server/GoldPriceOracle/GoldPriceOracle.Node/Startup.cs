using GoldPriceOracle.Configuration;
using GoldPriceOracle.Connection.Database;
using GoldPriceOracle.Infrastructure.Blockchain.Accounts;
using GoldPriceOracle.Infrastructure.DatabaseAccessServices;
using GoldPriceOracle.Infrastructure.Integration.ExternalAPI;
using GoldPriceOracle.Services.Interfaces;
using GoldPriceOracle.Services.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace GoldPriceOracle.Node
{
    public class Startup
    {
        private IConfiguration _cofiguration;

        public Startup(IConfiguration configuration)
        {
            _cofiguration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GoldPriceOracle.Node", Version = "v1" });
            });

            services.AddDbContext<OracleDbContext>(item => item.UseSqlServer(_cofiguration.GetConnectionString("OracleDb")));

            services.Configure<GoldAPIServiceOptions>(_cofiguration.GetSection("GoldAPIService"));

            services.AddScoped<OracleDbContext>();
            services.AddScoped<GoldAPIServiceOptions>();
            services.AddScoped<IHDWalletManagingService, HDWalletManagingService>();
            services.AddScoped<INodeDataDataAccessService, NodeDataAccessService>();
            services.AddScoped<IExternalGoldApiIntegrationService, ExternalGoldApiIntegrationService>();
            services.AddScoped<IAssetPriceHistoricalDataAccessService, AssetPriceHistoricalDataAccessService>();

            services.AddScoped<ISetupService, SetupService>();
            services.AddScoped<IInformationService, InformationService>();
            services.AddScoped<IIntegrationService, IntegrationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GoldPriceOracle.Node v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
