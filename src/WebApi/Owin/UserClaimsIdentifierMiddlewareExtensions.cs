using CosmosDb.Model.Interop;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Owin
{
    public static class UserClaimsIdentifierMiddlewareExtensions
    { 
        public static IApplicationBuilder UseClaimsIdentifier(this IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetRequiredService<ICosmosDbContext>();
            return app.UseMiddleware<CosmosDbUserClaimsIdentifierMiddleware>(context);
        }
    }
}
