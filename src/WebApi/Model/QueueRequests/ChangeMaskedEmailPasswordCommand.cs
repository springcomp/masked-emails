using Newtonsoft.Json;

namespace WebApi.Model.QueueRequests
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
