namespace MaskedEmails.Services.Storage.QueueRequests
{
    public class EnableMaskedEmailCommand : MaskedEmailCommand
    {
        public EnableMaskedEmailCommand()
        {
            Command = "enable-masked-email";
        }
    }
}