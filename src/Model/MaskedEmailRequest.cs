namespace Model
{
    public sealed class MaskedEmailRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PasswordHash { get; set; }
        public bool EnableForwarding { get; set; }
    }

    public sealed class UpdateMaskedEmailRequest
    { 
        public string Name { get; set; }
        public string Description { get; set; }
    }
}