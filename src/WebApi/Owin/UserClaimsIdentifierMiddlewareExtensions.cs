using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Owin
{
    public static class UserClaimsIdentifierMiddlewareExtensions
    { 
        public static IApplicationBuilder UseClaimsIdentifier(this IApplicationBuilder app)
        {
            var factory = app.ApplicationServices.GetRequiredService<IMaskedEmailsDbContextFactory>();
            return app.UseMiddleware<SQLiteUserClaimsIdentifierMiddleware>(factory);
        }
    }
}
