using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FunctionApp.Startup))]

namespace FunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureServices(builder.Services);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // retrieve IConfiguration instance if necessary
            // to configure the Function App

            var configuration = services.GetConfiguration();
        }
    }

    public static class IServiceCollectionConfigurationExtensions
    {
        public static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            return services.BuildServiceProvider().GetService<IConfiguration>();
        }
    }
}
