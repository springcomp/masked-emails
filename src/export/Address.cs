using Newtonsoft.Json;

public sealed class Address
{
    [JsonIgnore()]
    public int Id { get; set;  }
    [JsonProperty("name")]
    public string Name { get; set;  }
    [JsonProperty("description")]
    public string Description { get; set;  }
    [JsonProperty("emailAddress")]
    public string EmailAddress { get; set;  }
    [JsonIgnore()]
    public bool EnableForwarding { get; set;  }
    [JsonProperty("forwardToEmailAddress")]
    public string ForwardToEmailAddress { get; set;  }
    [JsonProperty("received")]
    public int Received { get; set;  }

    [JsonProperty("createdUtc")]
    public DateTime CreatedUtc { get; set;  }
    [JsonIgnore()]
    public string Profile_Id { get; set;  }
}
