using Newtonsoft.Json;

namespace MaskedEmails.Services.Storage.QueueRequests
{
    public class ChangeMaskedEmailPasswordCommand : MaskedEmailCommand
    {
        public ChangeMaskedEmailPasswordCommand()
        {
            Command = "change-masked-email-password";
        }
        [JsonProperty("password-hash")]
        public string PasswordHash { get; set; }
    }
}
