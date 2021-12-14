using CosmosDb.Model.Interop;

public static class SeedData
{
    public static void EnsureSeedData(IServiceProvider serviceProvider)
    {
        EnsureCosmosDbSeedData(serviceProvider).GetAwaiter().GetResult();
    }

    private static async Task EnsureCosmosDbSeedData(IServiceProvider serviceProvider)
    {
#if DEBUG
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ICosmosDbContext>();

        foreach (var profile in Config.GetProfiles())
        {
            var cosmosProfile = await context.GetProfile(profile.Id);
            if (cosmosProfile == null)
            {
                cosmosProfile = new CosmosDb.Model.Profile()
                {
                    Id = profile.Id,
                    EmailAddress = profile.EmailAddress,
                    DisplayName = profile.DisplayName,
                    ForwardingAddress = profile.ForwardingAddress,
                    CreatedUtc = DateTime.UtcNow,
                };
                foreach (var address in profile.Addresses)
                    cosmosProfile.Addresses.Add(new CosmosDb.Model.Address()
                    {
                        Name = address.Name,
                        Description = address.Description,
                        EmailAddress = address.EmailAddress,
                        EnableForwarding = address.EnableForwarding,
                        Received = address.Received,
                        CreatedUtc = address.CreatedUtc,
                    });

                await context.InserProfile(cosmosProfile);
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