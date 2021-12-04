using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using CosmosDb.Model.Interop;
using CosmosDb.Utils.Impl;
using CosmosDb.Utils.Interop;
using System.Linq;

namespace CosmosDb.Model
{
    public sealed class CosmosDbContext : ICosmosDbContext
    {
        private readonly ICosmosOperations operations_;

        private Database database_;
        private Container container_;

        public CosmosDbContext(ICosmosOperations operations)
        {
            operations_ = operations;
        }

        public async Task<Profile> GetProfile(string userId)
        {
            InitializeContext();

            try
            {
                Profile profile = await operations_.GetItemAsync<Profile>(container_, userId, userId);
                foreach (var address in profile.Addresses)
                    address.Profile = profile;
                return profile;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task InserProfile(Profile profile)
        {
            Database database = await operations_.CreateDatabaseIfNotExistsAsync(Constants.DatabaseId);
            Container container = await operations_.CreateContainerIfNotExistsAsync(database, Constants.ContainerName, Constants.PartitionKeyPath);

            await operations_.CreateItemAsync(container, profile, profile.Partition);
        }

        public async Task<Address> QueryAddress(string email)
        {
            await foreach (var page in QueryProfiles())
                foreach (var profile in page)
                {
                    var address = profile.Addresses.SingleOrDefault(a => a.EmailAddress == email);
                    if (address != null)
                    {
                        address.Profile = profile;
                        return address;
                    }
                }

            return null;
        }

        public IAsyncEnumerable<Page<Profile>> QueryProfiles()
        {
            InitializeContext();

            return operations_.QueryItemsAsync<Profile>(container_, Constants.QueryProfilesStatement);
        }

        public async Task UpdateProfile(Profile profile)
        {
            await operations_.ReplaceItemAsync(container_, profile, profile.Partition);
        }

        private void InitializeContext()
        {
            database_ = database_ ?? operations_.GetDatabase(Constants.DatabaseId);
            container_ = container_ ?? operations_.GetContainer(database_, Constants.ContainerName);
        }
    }
}
