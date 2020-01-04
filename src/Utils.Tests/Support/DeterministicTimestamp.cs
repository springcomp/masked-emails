using Utils.Interop;

namespace Utils.Tests.Support
{
    public sealed class DeterministicTimestamp : IUnixTime
    {
        public int Timestamp { get; set; }
        public ulong GetTimestamp()
        {
            return (ulong)Timestamp;
        }
    }
}