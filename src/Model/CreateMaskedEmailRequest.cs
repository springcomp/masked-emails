namespace Model
{
    public sealed class CreateMaskedEmailRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PasswordHash { get; set; }
        public bool EnableForwarding { get; set; }
    }
}