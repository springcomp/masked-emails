using System;
using Microsoft.Extensions.DependencyInjection;
using Data;

namespace WebApi
{
    public static class SeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
#if DEBUG
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MaskedEmailsDbContext>();
            context.Database.EnsureCreated();

            foreach (var profile in Config.GetProfiles())
            {
                if (context.Users.Find(profile.Id) == null)
                {
                    context.Users.Add(profile);
                    foreach (var address in profile.Addresses)
                        context.Addresses.Add(address);

                    context.SaveChanges();
                    Console.WriteLine($"Profile {profile.DisplayName} created.");
                }
                else
                {
                    Console.WriteLine($"Profile {profile.DisplayName} already created.");
                }
            }
#endif
        }
    }
}