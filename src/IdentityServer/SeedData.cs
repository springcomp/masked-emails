using System;
using System.Linq;
using IdentityModel;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpringComp.IdentityServer.TableStorage.Options;
using SpringComp.IdentityServer.TableStorage.Stores;

namespace IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetService<IServiceScopeFactory>().CreateScope();

            InitializeIdentityResources(scope.ServiceProvider);
            InitializeApiResources(scope.ServiceProvider);
            InitializeClients(scope.ServiceProvider);

            InitializeUsers(scope.ServiceProvider);
        }

        private static void InitializeUsers(IServiceProvider serviceProvider)
        {
            var userMgr = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            foreach (var user in Config.GetUsers())
            {
                var dbUser = userMgr.FindByNameAsync(user.Username).Result;
                if (dbUser != null)
                {
                    Console.WriteLine($"{user.Username} already exists with subject id \"{dbUser.Id}\"");
                    continue;
                }

                dbUser = new ApplicationUser {
                    Id = user.SubjectId,
                    UserName = user.Username,
                    Email = user.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Email)?.Value,
                    EmailConfirmed = true,
                };
                var result = userMgr.CreateAsync(dbUser, user.Password).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(dbUser, user.Claims).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                dbUser = userMgr.FindByNameAsync(user.Username).Result;
                Console.WriteLine($"{user.Username} created with subject id \"{dbUser.Id}\"");
            }
        }

        private static void InitializeApiResources(IServiceProvider services)
        {
            var options = services.GetRequiredService<IOptions<TableStorageConfigurationOptions>>();

            // seed API resources

            var apiResourceLogger = services.GetRequiredService<ILogger<ApiResourceTableStore>>();
            var apiResourceStore = new ApiResourceTableStore(options, apiResourceLogger);
            foreach (var apiResource in Config.GetApiResources())
                apiResourceStore.StoreAsync(apiResource).GetAwaiter().GetResult();
        }

        private static void InitializeIdentityResources(IServiceProvider services)
        {
            var options = services.GetRequiredService<IOptions<TableStorageConfigurationOptions>>();

            // seed Identity resources

            var identityResourceLogger = services.GetRequiredService<ILogger<IdentityResourceTableStore>>();
            var identityResourceStore = new IdentityResourceTableStore(options, identityResourceLogger);
            foreach (var identityResource in Config.GetIdentityResources())
                identityResourceStore.StoreAsync(identityResource).GetAwaiter().GetResult();
        }

        private static void InitializeClients(IServiceProvider services)
        {
            var options = services.GetRequiredService<IOptions<TableStorageConfigurationOptions>>();

            // seed Clients

            var clientLogger = services.GetRequiredService<ILogger<ClientStore>>();
            var clientStore = new ClientStore(options, clientLogger);
            foreach (var client in Config.GetClients())
                clientStore.StoreAsync(client).GetAwaiter().GetResult();
        }
    }
}
