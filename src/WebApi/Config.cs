using CosmosDb.Model;

#if DEBUG
public static class Config
{
    public static IEnumerable<Address> GetAddresses()
    {
        return GetProfiles().SelectMany(p => p.Addresses);
    }
    public static IEnumerable<Profile> GetProfiles()
    {
        var subjectIds = new[]
        {
                "a1118e83-92e6-4465-91e1-7595d060195c",
                "fe8ee8d2-5d47-463f-8ad3-bd65fc559246",
            };

        return
            new[]
            {
                    new Profile
                    {
                        Id = subjectIds[0],
                        DisplayName = "Alice",
                        EmailAddress = "alice@example.com",
                        CreatedUtc = DateTime.UtcNow,
                        ForwardingAddress = "alice@example.com",
                        Addresses = new List<Address>(new []
                        {
                            new Address
                            {
                                CreatedUtc = DateTime.UtcNow,
                                Name = "Sample",
                                Description = "ForwardedTo",
                                EmailAddress = "alice123@domain.com",
                                EnableForwarding = true,
                                Received = 0,
                            }
                        }),
                    },
                    new Profile
                    {
                        Id = subjectIds[1],
                        DisplayName = "Bob",
                        EmailAddress = "bobsmith@email.com",
                        CreatedUtc = DateTime.UtcNow,
                        ForwardingAddress = "bob@example.com",
                        Addresses = new List<Address>(new []
                        {
                            new Address
                            {
                                CreatedUtc = DateTime.UtcNow,
                                Name = "Sample",
                                Description = "ForwardedTo",
                                EmailAddress = "bob456@domain.com",
                                EnableForwarding = true,
                                Received = 0,
                            }
                        }),
                    },
            };
    }
}
#endif