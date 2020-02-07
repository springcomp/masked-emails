namespace Model
{
    public sealed class GetMaskedEmailPageResponse
    { 
        public int Count { get; set; }
        public int Total { get; set; }
        public string Cursor { get; set; }
        public MaskedEmail[] Addresses { get; set; }
    }
}