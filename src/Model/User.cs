using System;

namespace Model
{
    public sealed class UpdateProfileRequest
    { 
        public string DisplayName { get; set; }
        public string ForwardingAddress { get; set; }
    }
    public sealed class User
    {
        public string DisplayName { get; set; }
        public string ForwardingAddress { get; set; }

        public DateTime CreatedUtc { get; set; }
    }

    public sealed class UserClaim
    {
        public string Type { get; set; }
        public string Value { get; set;}
    }
}