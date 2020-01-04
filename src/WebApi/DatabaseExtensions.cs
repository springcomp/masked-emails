using System;
using System.Linq;
using Data;
using Data.Interop;
using Data.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi
{
    public static class DatabaseExtensions
    {
#if DEBUG
        public static IWebHost SeedInMemoryMaskedEmails(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<MaskedEmailsDbContext>();

                Initialize(services);
            }

            return host;
        }

        private static void Initialize(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<DbContextOptions<MaskedEmailsDbContext>>();
            using (var context = new MaskedEmailsDbContext(options))
                SeedMaskedEmails(context);
        }

        private static void SeedMaskedEmails(IMaskedEmailsDbContext context)
        {
            if (context.Users.Any())
                return;

            var profiles = Config
                    .GetProfiles()
                    .ToArray()
                ;

            context.Users.AddRange(GetProfiles(profiles));
            context.Addresses.AddRange(GetAddresses(profiles));

            context.SaveChanges();
        }

        private static Address[] GetAddresses(Profile[] profiles)
        {
            return profiles.SelectMany(p => p.Addresses)
                    .ToArray()
                ;
        }

        private static Profile[] GetProfiles(Profile[] profiles)
        {
            foreach (var profile in profiles)
            {
                foreach (var address in profile.Addresses)
                    address.Profile = profile;
            }

            return profiles;
        }
#endif
    }
}