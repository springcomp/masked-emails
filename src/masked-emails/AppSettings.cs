namespace masked_emails
{
    public sealed class AppSettings
    {
        public string Domain { get; set; } = "maskedbox.space";
    }

    public sealed class Endpoints
    { 
        public string Api { get; set; }
        public string Authority { get; set; }
        public string ClientSecret { get; set; }
    }
}