using System;

namespace Model
{
    public class MaskedEmail
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string EmailAddress { get; set; }
        public string ForwardToEmailAddress { get; set; }

        // statistics

        public int Received { get; set; }

        public DateTime CreatedUtc { get; set; }

        public void CloneTo(MaskedEmail target)
        {
            target.Name = Name;
            target.Description = Description;
            target.EmailAddress = EmailAddress;
            target.ForwardToEmailAddress = ForwardToEmailAddress;
            target.Received = Received;
            target.CreatedUtc = CreatedUtc;
        }
    }

    /// <summary>
    /// This is returned after a call to CreateMaskedEmail
    /// in order to display the generated password once.
    /// </summary>
    public sealed class MaskedEmailWithPassword : MaskedEmail
    {
        public static MaskedEmailWithPassword Clone(MaskedEmail model)
        {
            var cloned = new MaskedEmailWithPassword();
            model.CloneTo(cloned);
            return cloned;
        }
        public string Password { get; set; }
    }
}
