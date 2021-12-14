namespace MaskedEmails.Services.Configuration
{
    public class AppSettings
    {
        public string DomainName { get; set; }
        public string Authority { get; set; }
        public bool RequireHttpsMetadata { get; set; }
        public string Audience { get; set; }
        public int PasswordLength { get; set; } = 25;
    }
}