using CosmosDb.Model;
using CosmosDb.Model.Interop;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Model;

namespace WebApi.Owin
{
    public sealed class CosmosDbUserClaimsIdentifierMiddleware
    {
        private readonly RequestDelegate next_;
        private readonly ICosmosDbContext context_;

        public CosmosDbUserClaimsIdentifierMiddleware(RequestDelegate next, ICosmosDbContext context)
        {
            next_ = next ?? throw new ArgumentNullException(nameof(next));
            context_ = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.User != null && httpContext.User.Identity.IsAuthenticated)
            {
                const string emailAddress = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
                var email = httpContext.User.Claims.FirstOrDefault(c => c.Type == emailAddress);

                System.Diagnostics.Debug.Assert(email != null);
                if (email == null)
                    throw new ApplicationException();

                Profile authenticated = null;

                var profiles = context_.QueryProfiles();
                await foreach (var page in profiles)
                {
                    foreach (var profile in page)
                    {
                        if (String.Compare(profile.EmailAddress, email.Value, true) == 0)
                        {
                            authenticated = profile;
                            break;
                        }
                    }

                    if (authenticated != null)
                        break;
                }

                if (authenticated == null)
                    throw new ApplicationException();

                var appIdentity = httpContext.User.Identities.FirstOrDefault();
                appIdentity.AddClaim(new Claim(ClaimIdentifiers.UserId, authenticated.Id));
            }

            await next_(httpContext);
        }
    }
}
