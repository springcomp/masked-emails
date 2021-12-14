using System.Threading.Tasks;
using MaskedEmails.Services.Configuration.Extensions;
using MaskedEmails.Services.Interop;
using MaskedEmails.Services.Storage.QueueRequests;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MaskedEmails.Services
{
    public class MaskedEmailCommandQueueService : IMaskedEmailCommandService
    {
        private const string QueueName = "commands";

        private readonly CloudQueue queue_;

        public MaskedEmailCommandQueueService(IConfiguration configuration)
            : this(configuration.GetStorageConnectionString())
        {
        }
        public MaskedEmailCommandQueueService(string connectionString)
        {
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudQueueClient();
            queue_ = client.GetQueueReference(QueueName);
        }

        public Task QueueCommandAsync(MaskedEmailCommand command)
        {
            var text = JsonConvert.SerializeObject(command);
            return queue_.AddMessageAsync(new CloudQueueMessage(text));
        }
    }
}