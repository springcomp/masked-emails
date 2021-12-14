using System.Net.Http;
using System.Threading.Tasks;

namespace WebApi
{
    public interface IRequestToken
    { 
        Task<string> GetOAuthToken(HttpRequestMessage request);
    }
}
