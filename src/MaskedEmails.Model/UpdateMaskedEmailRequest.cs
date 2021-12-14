namespace MaskedEmails.Model
{
    public sealed class UpdateMaskedEmailRequest
    { 
        public string Name { get; set; }
        public string Description { get; set; }

        public string PasswordHash { get; set; }
    }
}