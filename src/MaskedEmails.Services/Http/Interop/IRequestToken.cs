using System.Net.Http;
using System.Threading.Tasks;

namespace MaskedEmails.Services.Http.Interop
{
    public interface IRequestToken
    {
        Task<string> GetOAuthToken(HttpRequestMessage request);
    }
}
