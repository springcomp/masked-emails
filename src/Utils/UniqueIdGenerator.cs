using System;
using System.Threading.Tasks;
using Utils.Interop;

namespace Utils
{
    public sealed class UniqueIdGenerator : IUniqueIdGenerator
    {
        private readonly ISequence sequence_;
        private readonly IUnixTime unix_;

        public UniqueIdGenerator(ISequence sequence)
            : this(sequence, new UnixTime())
        { }
        public UniqueIdGenerator(ISequence sequence, IUnixTime unix)
        {
            sequence_ = sequence;
            unix_ = unix;
        }

        public async Task<string> GenerateAsync()
        {
            // a unique identifier is basically a base36
            // string representation of a 32 bit number.
            // [sequence-64] + [time+32]

            var next = await sequence_.GetNextAsync();
            var nextBuffer = NetworkBitConverter.GetBytes(next);

            var time = unix_.GetTimestamp();
            var timeBuffer = NetworkBitConverter.GetBytes(time);

            var offset = 4;
            for (var index = 3; index >= 0; index--)
                if (timeBuffer[index] != 0x00) offset--;

            // try to ignore leading null bytes 

            var buffer = new byte[16];
            Array.Copy(nextBuffer, 0, buffer, offset, nextBuffer.Length);
            Array.Copy(timeBuffer, offset, buffer, offset + 8, timeBuffer.Length - offset);

            var number = NetworkBitConverter.ToUInt128(buffer, 0);
            var base36 = Base36
                    .NumberToBase36(number)
                    .ToLowerInvariant()
                ;

            return base36;
        }
    }
}