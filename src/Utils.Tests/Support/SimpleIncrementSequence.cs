using System.Threading.Tasks;
using Utils.Interop;

namespace Utils.Tests.Support
{
    public sealed class SimpleIncrementSequence : ISequence
    {
        private long seed_;

        public SimpleIncrementSequence(long seed)
        {
            seed_ = seed;
        }

        public Task<long> GetNextAsync()
        {
            return Task.FromResult(seed_++);
        }
    }
}