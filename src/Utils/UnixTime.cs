using System;
using Utils.Interop;

namespace Utils
{
    public class UnixTime : IUnixTime
    {
        public ulong GetTimestamp()
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var diff = DateTime.UtcNow - epoch;
            return Convert.ToUInt64(Math.Round(diff.TotalSeconds, MidpointRounding.AwayFromZero));
        }

    }
}