using Newtonsoft.Json;

namespace MaskedEmails.Services.Storage.QueueRequests
{
    public class CreateMaskedEmailCommand : MaskedEmailCommand
    {
        public CreateMaskedEmailCommand()
        {
            Command = "add-masked-email";
        }
        [JsonProperty("forward-to")]
        public string AlternateAddress { get; set; }
        [JsonProperty("password-hash")]
        public string PasswordHash { get; set; }
    }
}
