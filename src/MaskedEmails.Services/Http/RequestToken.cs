using IdentityModel.Client;
using MaskedEmails.Services.Configuration;
using MaskedEmails.Services.Http.Interop;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace MaskedEmails.Services.Http
{
    public sealed class RequestToken : IRequestToken
    {
        private readonly HttpClient client_;
        private readonly InboxApiSettings config_;

        private readonly ILogger logger_;

        public RequestToken(IOptions<InboxApiSettings> settings, IHttpClientFactory clientFactory, ILogger<RequestToken> logger)
        {
            config_ = settings.Value;
            client_ = clientFactory.CreateClient("inbox-api-oauth");

            logger_ = logger;
        }

        public async Task<string> GetOAuthToken(HttpRequestMessage request)
        {
            var clientId = config_.ClientId;
            var clientSecret = config_.ClientSecret;
            var audience = config_.Audience;
            var identityEndpoint = config_.IdentityEndpoint;
            var authority = config_.Authority;

            return (await RequestClientCredentials(clientId, clientSecret, audience, identityEndpoint, authority))
                .AccessToken
                ;
        }

        private async Task<TokenResponse> RequestClientCredentials(string clientId, string clientSecret, string audience, string identityEndpoint, string authority)
        {
            var request = new DiscoveryDocumentRequest()
            {
                Address = identityEndpoint,
                Policy = new DiscoveryPolicy()
                {
                    Authority = authority,
                    RequireHttps = true,
                    ValidateIssuerName = false,
                    ValidateEndpoints = false,
                },
            };
            var disco = await client_.GetDiscoveryDocumentAsync(request);
            if (disco.IsError)
            {
                logger_.LogCritical(disco.Error);
                return null;
            }

            var tokenResponse = await client_.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = clientId,
                    ClientSecret = clientSecret,

                    Resource = new[] { audience, },
                });

            if (tokenResponse.IsError)
            {
                logger_.LogError(tokenResponse.Error);
                return null;
            }

            return tokenResponse;
        }
    }
}
