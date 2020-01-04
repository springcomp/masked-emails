using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace masked_emails.Client
{
    public sealed partial class MaskedEmailsClient
    {
        class AuthenticatedHttpClientHandler : HttpClientHandler
        {
            private const string AuthenticationEndpoint = "https://masked-emails-identity.azurewebsites.net";

            private readonly Endpoints endpoints_;
            private readonly NetworkCredential credentials_;

            private string accessToken_ = null;

            public AuthenticatedHttpClientHandler(Endpoints endpoints, NetworkCredential credentials)
            {
                credentials_ = credentials;
                endpoints_ = endpoints;
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                // See if the request has an authorize header
                var auth = request.Headers.Authorization;
                if (auth != null)
                {
                    accessToken_ ??= await GetAccessTokenAsync().ConfigureAwait(false);
                    request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, accessToken_);
                }

                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            private async Task<string> GetAccessTokenAsync()
            {
                var server = endpoints_?.Authority ?? AuthenticationEndpoint;
                var username = credentials_.UserName;
                var password = credentials_.Password;

                var response = await RequestPasswordAccessTokenAsync(server, username, password);
                return response.AccessToken;
            }

            private async Task<TokenResponse> RequestPasswordAccessTokenAsync(string server, string username, string password)
            {
                var client = new HttpClient();
                var disco = await client.GetDiscoveryDocumentAsync(server);
                if (disco.IsError)
                    throw new HttpRequestException(disco.Error);

                var tokenResponse = await client.RequestPasswordTokenAsync(
                    new PasswordTokenRequest
                    {
                        Address = disco.TokenEndpoint,
                        ClientId = "client.ro",
                        ClientSecret = endpoints_.ClientSecret,

                        UserName = username,
                        Password = password,

                        Scope = "api",
                    });

                if (tokenResponse.IsError)
                    throw new HttpRequestException(tokenResponse.Error);

                return tokenResponse;
            }
        }
    }
}