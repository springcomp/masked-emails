using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using CosmosDb.Utils.Interop;
using CosmosDb.Utils.Logging;

namespace CosmosDb.Utils
{
    public sealed class CosmosRequestChargeOperations : CosmosOperations, ICosmosRequestChargeOperations, IRequestChargeAccumulator
    {
        private double requestCharges_ = 0.0D;
        private readonly ILogger logger_;

        public double RequestCharges => requestCharges_;

        public CosmosRequestChargeOperations(CosmosClient client)
            : this(client, null)
        {
        }
        public CosmosRequestChargeOperations(CosmosClient client, ILogger<CosmosRequestChargeOperations> logger)
            : base(client, (ILogger)logger)
        {
            logger_ = (ILogger)logger ?? NullLogger.Instance;
        }

        public override async Task<DatabaseResponse> CreateDatabaseIfNotExistsAsync(string databaseName)
        {
            return await CalcRequestCharges<DatabaseResponse>(
                nameof(ICosmosOperations.CreateDatabaseIfNotExistsAsync),
                async () => await base.CreateDatabaseIfNotExistsAsync(databaseName),
                r => r.RequestCharge
            );
        }

        public override async Task<ContainerResponse> CreateContainerIfNotExistsAsync(Database database, string containerName, string partitionPath)
        {
            return await CalcRequestCharges<ContainerResponse>(
                nameof(ICosmosOperations.CreateContainerIfNotExistsAsync),
                async () => await base.CreateContainerIfNotExistsAsync(database, containerName, partitionPath),
                r => r.RequestCharge
                );
        }

        public override async Task<ItemResponse<T>> GetItemAsync<T>(Container container, string partition, string id) 
        {
            return await CalcRequestCharges<ItemResponse<T>>(
                nameof(ICosmosOperations.GetItemAsync),
                async () => await base.GetItemAsync<T>(container, partition, id),
                r => r.RequestCharge
            );
        }

        public override async Task<ItemResponse<T>> CreateItemAsync<T>(Container container, T item, string partition) 
        {
            return await CalcRequestCharges<ItemResponse<T>>(
                nameof(ICosmosOperations.CreateItemAsync),
                async () => await base.CreateItemAsync(container, item, partition),
                r => r.RequestCharge
            );
        }

        public override async Task<ItemResponse<T>> ReplaceItemAsync<T>(Container container, T item, string partition) 
        {
            return await CalcRequestCharges<ItemResponse<T>>(
                nameof(ICosmosOperations.CreateItemAsync),
                async () => await base.ReplaceItemAsync(container, item, partition),
                r => r.RequestCharge
            );
        }

        public override CosmosDbQuery<T> QueryItemsAsync<T>(Container container, string statement)
        {
            return new CosmosDbQuery<T>(container, statement, this as IRequestChargeAccumulator);
        }

        private async Task<T> CalcRequestCharges<T>(string name, Func<Task<T>> function, Func<T, double> getRequestCharge)
        {
            T response = default(T);
            try
            {
                response = await function();
                return response;
            }
            finally
            {
                if (response != null)
                {
                    var requestCharge = getRequestCharge(response);
                    logger_.TraceRequestCharge(name, requestCharge);
                    (this as IRequestChargeAccumulator).AccumulateRequestCharges(name, requestCharge);
                }
            }
        }

        void IRequestChargeAccumulator.AccumulateRequestCharges(string name, double requestCharge)
        {
            logger_.TraceRequestCharge(name, requestCharge);
            requestCharges_ += requestCharge;
        }
    }
}
