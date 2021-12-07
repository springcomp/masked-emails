using CosmosDb.Model.Configuration;
using CosmosDb.Model.Interop;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
namespace CosmosDb.Model
{
    public sealed class CosmosDbClientFactory : ICosmosDbClientFactory
    {
        private readonly CosmosDbSettings appSettings_;
        private readonly ILogger logger_;

        // CosmosDb client must be a singleton
        private static CosmosClient client_;
        public CosmosDbClientFactory(IOptions<CosmosDbSettings> appSettings, ILogger<CosmosDbClientFactory> logger)
        {
            appSettings_ = appSettings.Value;
            logger_ = logger;
        }

        public CosmosClient CreateClient()
        {
            client_ = client_ ?? InitializeCosmosDbClient();
            return client_;
        }
        private CosmosClient InitializeCosmosDbClient()
        {
            var endpoint = appSettings_.EndpointUri;
            var primaryKey = appSettings_.PrimaryKey;
            var ignoreSslCertificate = appSettings_.IgnoreSslServerCertificateValidation;

            logger_.LogInformation("Initializing CosmosDb client:");
            logger_.LogInformation($"Endpoint: {endpoint}.");
            logger_.LogInformation($"PrimaryKey: {primaryKey.Substring(0, 4)}***REDACTED***.");
            logger_.LogInformation($"IgnoreSslServerCertificate: { ignoreSslCertificate}.");

            CosmosClientOptions options = ignoreSslCertificate ? GetUnsafeCosmosClientOptions() : new CosmosClientOptions();
            var client = new CosmosClient(endpoint, primaryKey, options);
            return client;
        }

        private bool unsafeOptionsWarningDisplayed = false;
        private CosmosClientOptions GetUnsafeCosmosClientOptions()
        {
            return new CosmosClientOptions
            {
                HttpClientFactory = () =>
                {
                    HttpMessageHandler httpMessageHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (req, cert, chain, errs) =>
                        {
                            if (!unsafeOptionsWarningDisplayed)
                            {
                                logger_.LogWarning("CosmosDb: ignoring untrusted Ssl server certificate.");
                                unsafeOptionsWarningDisplayed = true;
                            }
                            return HttpClientHandler.DangerousAcceptAnyServerCertificateValidator(req, cert, chain, errs);
                        }
                    };
                    return new HttpClient(httpMessageHandler);
                },
                ConnectionMode = ConnectionMode.Gateway,
            };
        }
    }
}
