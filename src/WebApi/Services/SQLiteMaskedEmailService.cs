using System.Linq;
using System.Threading.Tasks;
using Data.Interop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Model;
using WebApi.Model;
using WebApi.Services.Interop;

namespace WebApi.Services
{
    using Profile = Data.Model.Profile;
    using Address = Data.Model.Address;
    public class SQLiteMaskedEmailService : IMaskedEmailService
    {
        private readonly IMaskedEmailsDbContext context_;

        public SQLiteMaskedEmailService(IMaskedEmailsDbContext context)
        {
            context_ = context;
        }

        public async Task<MaskedEmail> GetMaskedEmail(string email)
        {
            var address = await GetAddresses()
                .SingleOrDefaultAsync(a => a.EmailAddress == email)
                ;

            if (address == null)
                throw Error.NoSuchMaskedEmailAddress(email);

            return address.ToModel();
        }

        public async Task<MaskedEmail> IncrementReceiveCount(string email)
        {
            var address = await GetAddressesForUpdate()
                .SingleOrDefaultAsync(a => a.EmailAddress == email)
                ;

            if (address == null)
                throw Error.NoSuchMaskedEmailAddress(email);

            address.Received++;
            await context_.SaveChangesAsync();

            return address.ToModel();
        }

        #region Implementation

        private IIncludableQueryable<Address, Profile> QueryAddresses()
        {
            return context_.Addresses
                .Include(a => a.Profile)
                ;
        }

        private IQueryable<Address> GetAddresses()
        {
            return QueryAddresses()
                    .AsNoTracking()
                ;
        }

        private IQueryable<Address> GetAddressesForUpdate()
        {
            return QueryAddresses()
                ;
        }

        #endregion
    }
}