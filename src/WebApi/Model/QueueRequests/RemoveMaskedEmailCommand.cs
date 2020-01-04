namespace WebApi.Model.QueueRequests
{
    public class RemoveMaskedEmailCommand : MaskedEmailCommand
    {
        public RemoveMaskedEmailCommand()
        {
            Command = "remove-masked-email";
        }
    }
}