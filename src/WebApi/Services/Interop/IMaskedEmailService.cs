using System.Threading.Tasks;
using Model;
using WebApi.Model;

namespace WebApi.Services.Interop
{
    public interface IMaskedEmailService
    {
        Task<MaskedEmail> GetMaskedEmail(string email);
        Task<MaskedEmail> IncrementReceiveCount(string email);
    }
}