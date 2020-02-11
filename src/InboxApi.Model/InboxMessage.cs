using System;

namespace InboxApi.Model
{
    public class InboxMessageSpec
    {
        public string Location { get; set; }
        public DateTime ReceivedUtc { get; set; }
        public string Subject { get; set; }
        public EmailAddress Sender { get; set; }
    }

    public sealed class InboxMessage : InboxMessageSpec { 
        public string HtmlBody { get; set; }
        public string TextBody { get; set; }
    }
}
