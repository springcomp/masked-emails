namespace WebApi.Configuration
{
    public class AppSettings
    {
        public string DomainName { get; set; }
        public string Authority { get; set; }
        public bool RequireHttpsMetadata { get; set; }
        public string Audience { get; set; }
        public int HttpsPort { get; set; } = 443;
        public int PasswordLength { get; set; } = 25;
    }
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