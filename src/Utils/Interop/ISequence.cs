using System.Threading.Tasks;

namespace Utils.Interop
{
    public interface ISequence
    {
        Task<long> GetNextAsync();
    }
}