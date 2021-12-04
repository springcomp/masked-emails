using Model;

namespace WebApi.Model
{
    public static class ProfileExtensions
    {
        public static User ToModel(this CosmosDb.Model.Profile profile)
        { 
            return new User
            {
                DisplayName = profile.DisplayName,
                ForwardingAddress = profile.ForwardingAddress,
                CreatedUtc = profile.CreatedUtc,
            };
        }
    }
}
