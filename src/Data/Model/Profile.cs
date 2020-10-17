using System;
using System.Collections.Generic;

namespace Data.Model
{
    public class Profile
    {
        public Profile()
        {
            Addresses = new HashSet<Address>();
        }

        public string Id { get; set; }
        public string EmailAddress { get; set; }

        public string DisplayName { get; set; }
        public string ForwardingAddress { get; set; }

        public DateTime CreatedUtc { get; set; }

        // navigation properties
        public virtual ICollection<Address> Addresses { get; set; }
    }
}