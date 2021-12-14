namespace MaskedEmails.Services.Storage.QueueRequests
{
    public class RemoveMaskedEmailCommand : MaskedEmailCommand
    {
        public RemoveMaskedEmailCommand()
        {
            Command = "remove-masked-email";
        }
    }
}