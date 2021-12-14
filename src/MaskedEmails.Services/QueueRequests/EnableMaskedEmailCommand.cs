namespace WebApi.Model.QueueRequests
{
    public class EnableMaskedEmailCommand : MaskedEmailCommand
    {
        public EnableMaskedEmailCommand()
        {
            Command = "enable-masked-email";
        }
    }
}