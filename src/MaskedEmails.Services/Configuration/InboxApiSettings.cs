namespace MaskedEmails.Services.Configuration
{
    public class InboxApiSettings
    {
        public string Endpoint { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Audience { get; set; }
        public string IdentityEndpoint { get; set; }
        public string Authority { get; set; }
    }
}