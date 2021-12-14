namespace MaskedEmails.Services.Storage.QueueRequests
{
    public class DisableMaskedEmailCommand : MaskedEmailCommand
    {
        public DisableMaskedEmailCommand()
        {
            Command = "disable-masked-email";
        }
    }
}