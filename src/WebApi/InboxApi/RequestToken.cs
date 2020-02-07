using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.Configuration;

namespace WebApi
{
    public sealed class RequestToken : IRequestToken
    {
        private readonly HttpClient client_;
        private readonly IOptions<InboxApiSettings> config_;

        private readonly ILogger logger_;

        public RequestToken(IOptions<InboxApiSettings> settings, IHttpClientFactory clientFactory, ILogger<RequestToken> logger)
        {
            config_ = settings;
            client_ = clientFactory.CreateClient("inbox-api-oauth");

            logger_ = logger;
        }

        public async Task<string> GetOAuthToken(HttpRequestMessage request)
        {
            var clientId = config_.Value.ClientId;
            var clientSecret = config_.Value.ClientSecret;

            return (await RequestClientCredentials(clientId, clientSecret, scopes: "inbox"))
                .AccessToken
                ;
        }

        private async Task<TokenResponse> RequestClientCredentials(string clientId, string clientSecret, string scopes)
        {
            var disco = await client_.GetDiscoveryDocumentAsync(client_.BaseAddress.OriginalString);
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

                    Scope = scopes,
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
