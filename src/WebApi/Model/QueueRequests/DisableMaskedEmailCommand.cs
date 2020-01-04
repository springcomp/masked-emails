
namespace WebApi.Model.QueueRequests
{
    public class DisableMaskedEmailCommand : MaskedEmailCommand
    {
        public DisableMaskedEmailCommand()
        {
            Command = "disable-masked-email";
        }
    }
}