using System.Threading.Tasks;

namespace Utils.Interop
{
    public interface IUniqueIdGenerator
    {
        Task<string> GenerateAsync();
    }
}