using System.Net.Http;
using System.Threading.Tasks;

namespace MaskedEmails.Inbox.Interop
{
    public interface IRequestToken
    {
        Task<string> GetOAuthToken(HttpRequestMessage request);
    }
}
