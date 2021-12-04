using System;
using System.Collections.Generic;
using CosmosDb.Utils.Interop;
using Newtonsoft.Json;

namespace CosmosDb.Model
{
    public class Profile : ICosmosDbItem
    {
        public Profile()
        {
            Addresses = new List<Address>();
        }

        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }
        [JsonProperty("displayName")]

        public string DisplayName { get; set; }
        [JsonProperty("forwardingAddress")]
        public string ForwardingAddress { get; set; }

        [JsonProperty("createdUtc")]
        public DateTime CreatedUtc { get; set; }

        [JsonProperty("addresses")]
        public List<Address> Addresses { get; set; }

        [JsonIgnore()]
        public string Partition => Id;
    }
}
