public sealed class Profile {
    public string Id { get; set; }
    public string EmailAddress { get; set; }
    public string DisplayName { get; set; }
    public string ForwardingAddress { get; set; }
    public DateTime CreatedUtc { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, EmailAddress={EmailAddress}, ForwardingAddress={ForwardingAddress}";
    }
}
