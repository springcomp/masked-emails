using Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Model;

namespace WebApi.Owin
{
    public sealed class UserClaimsIdentifierMiddleware
    {
        private readonly RequestDelegate next_;
        private readonly IMaskedEmailsDbContextFactory factory_;

        public UserClaimsIdentifierMiddleware(RequestDelegate next, IMaskedEmailsDbContextFactory factory)
        {
            next_ = next ?? throw new ArgumentNullException(nameof(next));
            factory_ = factory ?? throw new ArgumentNullException(nameof(factory));
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

                using (var context_ = factory_.CreateDbContext())
                {
                    var profile = context_.Users.ToList().Single(p => String.Compare(p.EmailAddress, email.Value, true) == 0);
                    if (profile == null)
                        throw new ApplicationException();

                    var appIdentity = httpContext.User.Identities.FirstOrDefault();
                    appIdentity.AddClaim(new Claim(ClaimIdentifiers.UserId, profile.Id));
                }
            }

            await next_(httpContext);
        }
    }
}
