using Microsoft.Extensions.Configuration;

namespace IdentityServer.Services
{
    public static class ConnectionStringHelper
    {
        public static string GetStorageConnectionString(IConfiguration configuration)
        {
            return configuration["TableStorage:IdentityServerStore"];
        }
    }
}
