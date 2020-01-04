namespace Utils.Interop
{
    public interface IUnixTime
    {
        /// <summary>
        /// Returns the whole number of seconds
        /// elapsed since 1st january 1970.
        /// </summary>
        /// <returns></returns>
        ulong GetTimestamp();
    }
}