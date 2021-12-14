using Newtonsoft.Json;

namespace WebApi.Model.QueueRequests
{
    public class MaskedEmailCommand
    {
        [JsonProperty("command")]
        public string Command { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
    }
}