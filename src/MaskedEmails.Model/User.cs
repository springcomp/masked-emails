using System;

namespace MaskedEmails.Model
{
    public sealed class User
    {
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public string ForwardingAddress { get; set; }

        public DateTime CreatedUtc { get; set; }
    }
}