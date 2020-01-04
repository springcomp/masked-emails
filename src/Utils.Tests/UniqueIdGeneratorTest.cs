using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Utils.Numerics;
using Utils.Tests.Support;

namespace Utils.Tests
{
    [TestFixture]
    public class UniqueIdGeneratorTest
    {
        [Test]
        public async Task UniqueIdGenerator_GenerateAsync()
        {
            var sequence = new SimpleIncrementSequence(42424242);
            var timestamp = new DeterministicTimestamp { Timestamp = 1568976989, };

            var generator = new UniqueIdGenerator(sequence, timestamp);

            for (var index = 0; index < 100; index++)
            {
                var unique = await generator.GenerateAsync();
                System.Diagnostics.Debug.WriteLine(unique);
            }
        }
    }
}