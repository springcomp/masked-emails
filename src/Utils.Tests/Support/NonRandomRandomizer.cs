using Utils.Interop;

namespace Utils.Tests.Support
{
    public sealed class NonRandomRandomizer : IRandom
    {
        public int index = 0;
        public int[] NextValues { get; set; }
        public int Next(int maxValue)
        {
            return NextValues[index++ % NextValues.Length];
        }
    }
}