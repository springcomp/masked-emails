using Model;

namespace MaskedEmails.Services.Model
{
    public static class ProfileExtensions
    {
        public static User ToModel(this CosmosDb.Model.Profile profile)
        {
            return new User
            {
                DisplayName = profile.DisplayName,
                EmailAddress = profile.EmailAddress,
                ForwardingAddress = profile.ForwardingAddress,
                CreatedUtc = profile.CreatedUtc,
            };
        }
    }
}
