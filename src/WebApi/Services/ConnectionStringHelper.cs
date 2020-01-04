using Microsoft.Extensions.Configuration;

namespace WebApi.Services
{
    public static class ConnectionStringHelper
    {
        public static string GetStorageConnectionString(IConfiguration configuration)
        {
            return configuration["TableStorage:ConnectionString"];
        }
    }
}