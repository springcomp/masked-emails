using Newtonsoft.Json;

namespace IdentityServer.Services
{
    internal class SendEmailCommand
    {
        [JsonProperty("command")]
        public string Command { get; set; } = "send-email";

        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("subject")]
        public string Subject { get; set; }
        [JsonProperty("message")]
        public string HtmlMessage { get; set; }
    }
}
