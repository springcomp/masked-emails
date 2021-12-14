using MaskedEmails.Inbox.Interop;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MaskedEmails.Inbox.Http
{
    public class AuthenticatedParameterizedHttpClientHandler : DelegatingHandler
    {
        readonly IRequestToken getToken_;

        public AuthenticatedParameterizedHttpClientHandler(IRequestToken getToken)
        {
            getToken_ = getToken ?? throw new ArgumentNullException(nameof(getToken));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // See if the request has an authorize header
            var auth = request.Headers.Authorization;
            if (auth != null)
            {
                var token = await getToken_.GetOAuthToken(request).ConfigureAwait(false);
                request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, token);
            }

            var content = await request.Content?.ReadAsStringAsync();

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
