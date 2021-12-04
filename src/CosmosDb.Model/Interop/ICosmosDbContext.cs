using CosmosDb.Utils.Impl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosDb.Model.Interop
{
    public interface ICosmosDbContext
    {
        Task<Address> QueryAddress(string email);
        IAsyncEnumerable<Page<Profile>> QueryProfiles();
        Task<Profile> GetProfile(string userId);
        Task UpdateProfile(Profile profile);
        Task InserProfile(Profile profile);
    }
}
