using System.Threading.Tasks;
using CosmosDb.Model.Interop;
using MaskedEmails.Services;
using MaskedEmails.Services.Model;
using Model;
using WebApi.Services.Interop;

namespace WebApi.Services
{
    public class CosmosDbMaskedEmailService : IMaskedEmailService
    { 
        private readonly ICosmosDbContext context_;

        public CosmosDbMaskedEmailService(ICosmosDbContext context)
        {
            context_ = context;
        }

        public async Task<MaskedEmail> GetMaskedEmail(string email)
        {
            var address = await context_.QueryAddress(email);
            if (address == null)
                throw Error.NoSuchMaskedEmailAddress(email);

            return address.ToModel();
        }

        public async Task<MaskedEmail> IncrementReceiveCount(string email)
        {
            var address = await context_.QueryAddress(email);
            if (address == null)
                throw Error.NoSuchMaskedEmailAddress(email);

            System.Diagnostics.Debug.Assert(address.Profile != null);

            address.Received++;
            await context_.UpdateProfile(address.Profile);

            return address.ToModel();
        }
    }
}