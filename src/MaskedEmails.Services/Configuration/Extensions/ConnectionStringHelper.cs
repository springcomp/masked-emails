using Microsoft.Extensions.Configuration;

namespace MaskedEmails.Services.Configuration.Extensions
{
    public static class ConnectionStringHelper
    {
        public static string GetStorageConnectionString(this IConfiguration configuration)
        {
            return configuration["TableStorage:ConnectionString"];
        }
    }
}