using Model;

namespace WebApi.Model
{
    public static class AddressExtensions
    {
        public static MaskedEmail ToModel(this CosmosDb.Model.Address address)
        {
            // TODO: create a new field ForwardToEmailAddress in Address record

            return new MaskedEmail
            {
                Name = address.Name,
                Description = address.Description,
                EmailAddress = address.EmailAddress,
                ForwardToEmailAddress = address.EnableForwarding ? address.Profile.ForwardingAddress : null,

                Received = address.Received,
                CreatedUtc = address.CreatedUtc,
            };
        }
    }
}