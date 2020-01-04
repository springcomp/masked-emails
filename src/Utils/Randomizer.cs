using System;
using Utils.Interop;

namespace Utils
{
    /// <summary>
    /// Wraps .NET Random class.
    /// </summary>
    public sealed class Randomizer : IRandom
    {
        private static readonly Random random_ 
                = new Random(GetRandomSeed())
            ;
        public int Next(int maxValue)
        {
            return random_.Next(maxValue);
        }
        private static int GetRandomSeed()
        {
            const int ul = 0x0000000007FFFFFFF;
            var seed = DateTime.UtcNow.Ticks;
            var masked = seed & ul;
            return Convert.ToInt32(masked);
        }
    }
}