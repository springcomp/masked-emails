using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebApi.Model.QueueRequests;
using WebApi.Services.Interop;

namespace WebApi.Services
{
    public class MaskedEmailCommandQueueService : IMaskedEmailCommandService
    {
        private const string QueueName = "commands";

        private readonly CloudQueue queue_;

        public MaskedEmailCommandQueueService(IConfiguration configuration)
            : this(ConnectionStringHelper.GetStorageConnectionString(configuration))
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