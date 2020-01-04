using System;
using System.Collections.Generic;

namespace Data.Model
{
    public sealed class Profile
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }
        public string ForwardingAddress { get; set; }

        public DateTime CreatedUtc { get; set; }

        // navigation properties
        public ICollection<Address> Addresses { get; set; }
    }
}