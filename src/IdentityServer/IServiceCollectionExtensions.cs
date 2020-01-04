using IdentityServer.Data;
using IdentityServer.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ElCamino.AspNetCore.Identity.AzureTable.Model;

using IdentityRole =  ElCamino.AspNetCore.Identity.AzureTable.Model.IdentityRole;

namespace IdentityServer
{
    public static class ServiceCollectionExtensions
    {
        public static IdentityBuilder AddAspNetIdentity(this IServiceCollection services, string connectionString, IConfiguration configuration)
        {
            var builder = services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                    {
                        options.SignIn.RequireConfirmedAccount = true;
                        options.User.RequireUniqueEmail = true;

                        options.Password.RequireDigit = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                    })
                    .AddAzureTableStores<ApplicationDbContext>(() => new IdentityConfiguration
                    {
                        TablePrefix = configuration
                            .GetSection("IdentityAzureTable:IdentityConfiguration:TablePrefix").Value,
                        StorageConnectionString = connectionString,
                        LocationMode = configuration
                            .GetSection("IdentityAzureTable:IdentityConfiguration:LocationMode").Value
                    })
                    .AddDefaultTokenProviders()
                    .CreateAzureTablesIfNotExists<ApplicationDbContext>()
                ;

            return builder;
        }
        public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services, string connectionString)
        {
            var builder = services.AddIdentityServer(
                        options =>
                        {
                            options.Events.RaiseErrorEvents = true;
                            options.Events.RaiseInformationEvents = true;
                            options.Events.RaiseFailureEvents = true;
                            options.Events.RaiseSuccessEvents = true;
                        })
                    .AddSecretParser<JwtBearerClientAssertionSecretParser>()
                    .AddSecretValidator<PrivateKeyJwtSecretValidator>()
                    .AddConfigurationStore(connectionString)
                    .AddOperationalStore(connectionString, options =>
                    {
                        options.EnableTokenCleanup = true;
#if DEBUG
                        options.TokenCleanupInterval = 120;
#endif
                    })
                    .AddAspNetIdentity<ApplicationUser>()
                ;

            return builder;
        }
    }
}