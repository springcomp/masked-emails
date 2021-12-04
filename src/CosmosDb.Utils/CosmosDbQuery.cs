using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using CosmosDb.Utils.Impl;
using CosmosDb.Utils.Interop;

namespace CosmosDb.Utils
{
    public sealed class CosmosDbQuery<T> : IAsyncEnumerable<Page<T>> where T : ICosmosDbItem
    {
        private readonly Container container_;
        private readonly QueryDefinition query_;

        private readonly IRequestChargeAccumulator requestCharges_;

        internal CosmosDbQuery(Container container, string query, IRequestChargeAccumulator requestCharges = null)
        {
            container_ = container;
            query_ = new QueryDefinition(query);

            requestCharges_ = requestCharges;
        }

        public IAsyncEnumerator<Page<T>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new Enumerator(container_, query_, requestCharges_, cancellationToken);
        }

        private sealed class Enumerator : IAsyncEnumerator<Page<T>>
        {
            private readonly Container container_;
            private readonly QueryDefinition query_;
            private readonly IRequestChargeAccumulator requestCharges_;
            private readonly CancellationToken cancellationToken_;

            private FeedIterator<T> iterator_ = null;
            private Page<T> current_ = null;

            public Enumerator(Container container, QueryDefinition query, IRequestChargeAccumulator requestCharges, CancellationToken cancellationToken)
            {
                container_ = container;
                query_ = query;
                requestCharges_ = requestCharges;
                cancellationToken_ = cancellationToken;
            }

            public Page<T> Current
                => current_;

            public async ValueTask<bool> MoveNextAsync()
            {
                iterator_ = iterator_ ?? container_.GetItemQueryIterator<T>(query_);
                var result = iterator_.HasMoreResults;
                if (result)
                {
                    var response = await iterator_.ReadNextAsync(cancellationToken_);
                    requestCharges_?.AccumulateRequestCharges(nameof(FeedIterator<T>.ReadNextAsync), response.RequestCharge);
                    current_ = new Page<T>(response);
                }
                return result;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            public ValueTask DisposeAsync()
            {
                return new ValueTask(Task.CompletedTask);
            }
        }
    }
}