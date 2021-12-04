using Newtonsoft.Json;
using System;

namespace CosmosDb.Model
{
    public class Address
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }
        [JsonProperty("enableForwarding")]
        public bool EnableForwarding { get; set; }

        // statistics

        [JsonProperty("received")]
        public int Received { get; set; }

        [JsonProperty("createdUtc")]
        public DateTime CreatedUtc { get; set; }

        // this field will be populated 
        // to link back to its parent Profile object
        [JsonIgnore()]
        public Profile Profile { get; set; }
    }
}
