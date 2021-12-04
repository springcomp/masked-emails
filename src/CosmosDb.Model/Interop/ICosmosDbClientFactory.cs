using Microsoft.Azure.Cosmos;

namespace CosmosDb.Model.Interop
{
    public interface ICosmosDbClientFactory
    {
        CosmosClient CreateClient();
    }
}
