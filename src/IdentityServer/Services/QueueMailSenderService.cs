using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    public class QueueMailSenderService : IEmailSender
    {
        private const string QueueName = "commands";

        private readonly CloudQueue queue_;

        public QueueMailSenderService(IConfiguration configuration)
            : this(ConnectionStringHelper.GetStorageConnectionString(configuration))
        {
        }
        public QueueMailSenderService(string connectionString)
        {
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudQueueClient();
            queue_ = client.GetQueueReference(QueueName);
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return QueueCommandAsync(new SendEmailCommand {
                Address = email,
                Subject = subject,
                HtmlMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(htmlMessage)),
            });
        }
        private Task QueueCommandAsync(SendEmailCommand command)
        {
            var text = JsonConvert.SerializeObject(command);
            return queue_.AddMessageAsync(new CloudQueueMessage(text));
        }
    }
}
