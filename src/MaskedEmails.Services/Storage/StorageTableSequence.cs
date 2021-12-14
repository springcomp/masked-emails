using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MaskedEmails.Services.Configuration.Extensions;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using Utils.Interop;

namespace MaskedEmails.Services.Storage
{
    public sealed class StorageTableSequence : ISequence
    {
        private static bool TableCreated = false;

        private readonly CloudTable table_;

        private const string TableName = "SequenceNumber";
        private const long Seed = 1987654321L;

        public StorageTableSequence(IConfiguration configuration)
            : this(configuration.GetStorageConnectionString())
        {
        }
        public StorageTableSequence(string connectionString)
        {
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudTableClient();
            table_ = client.GetTableReference(TableName);
        }
        public async Task<long> GetNextAsync()
        {
            await EnsureTableCreatedOnceAsync();

            bool succeeded;

            var current = InitialSequenceNumber;

            do
            {
                try
                {
                    current = await GetCurrentSequenceAsync();
                    if (current == InitialSequenceNumber || current.Number == 0L)
                    {
                        current.Number = Seed;
                        await InsertSequenceNumberAsync(current);
                    }

                    current.Number += 1L;
                    await UpdateSequenceNumberAsync(current);
                    succeeded = true;
                }
                catch (StorageException e)
                {
                    var statusCode = e.RequestInformation.HttpStatusCode;
                    if (statusCode == (int)HttpStatusCode.PreconditionFailed || statusCode == (int)HttpStatusCode.Conflict)
                        succeeded = false;
                    else
                    {
                        throw;
                    }
                }


            } while (!succeeded)
                ;

            return current.Number;
        }

        private async Task EnsureTableCreatedOnceAsync()
        {
            if (!TableCreated)
            {
                await table_.CreateIfNotExistsAsync();
                TableCreated = true;
            }
        }

        private async Task InsertSequenceNumberAsync(ITableEntity current)
        {
            var operation = TableOperation.Insert(current);
            await table_.ExecuteAsync(operation);
        }

        private async Task UpdateSequenceNumberAsync(ITableEntity current)
        {
            var operation = TableOperation.Replace(current);
            await table_.ExecuteAsync(operation);
        }

        private async Task<SequenceNumber> GetCurrentSequenceAsync()
        {
            var current = InitialSequenceNumber;

            var query = new TableQuery<SequenceNumber>().Where(
                LogicalAnd(
                    WhereEqual("PartitionKey", InitialSequenceNumber.PartitionKey),
                    WhereEqual("RowKey", InitialSequenceNumber.RowKey)
                )
            );

            var continuationToken = new TableContinuationToken();
            var segment = await table_.ExecuteQuerySegmentedAsync(query, continuationToken);
            if (segment.Results.Count != 0)
                current = segment.Results[0];

            return current;
        }

        public static string LogicalAnd(string left, string right)
        {
            return TableQuery.CombineFilters(left, TableOperators.And, right);
        }
        public static string WhereEqual(string propertyName, string value)
        {
            return TableQuery.GenerateFilterCondition(propertyName, QueryComparisons.Equal, value);
        }

        private static readonly SequenceNumber InitialSequenceNumber = new SequenceNumber();

        internal sealed class SequenceNumber : TableEntity
        {
            public SequenceNumber()
                : base("Sequence", "Number")
            {
            }

            [IgnoreProperty]
            public long Number { get; set; }

            public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
            {
                base.ReadEntity(properties, operationContext);
                if (properties.ContainsKey(nameof(Number)))
                    Number = properties[nameof(Number)].Int64Value.GetValueOrDefault();
            }

            public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
            {
                var collection = base.WriteEntity(operationContext);
                collection.Add(nameof(Number), new EntityProperty(Number));

                return collection;
            }
        }
    }
}