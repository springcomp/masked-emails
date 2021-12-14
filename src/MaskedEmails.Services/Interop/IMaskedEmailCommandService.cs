using System.Threading.Tasks;
using WebApi.Model.QueueRequests;

namespace WebApi.Services.Interop
{
    public interface IMaskedEmailCommandService
    {
        Task QueueCommandAsync(MaskedEmailCommand command);
    }
}