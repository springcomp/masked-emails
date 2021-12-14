using Newtonsoft.Json;

namespace MaskedEmails.Services.Storage.QueueRequests
{
    public class MaskedEmailCommand
    {
        [JsonProperty("command")]
        public string Command { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
    }
}