using Newtonsoft.Json;

public sealed class User
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("displayName")]
    public string DisplayName { get; set; }
    [JsonProperty("emailAddress")]
    public string EmailAddress { get; set; }
    [JsonProperty("createdUtc")]
    public DateTime CreatedUtc { get; set; }
    [JsonProperty("forwardingAddress")]
    public string ForwardingAddress { get; set; }
    [JsonProperty("addresses")]
    public Address[] Addresses { get; set; }
}
