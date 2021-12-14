using MaskedEmails.Services.Storage.QueueRequests;
using System.Threading.Tasks;

namespace MaskedEmails.Services.Interop
{
    public interface IMaskedEmailCommandService
    {
        Task QueueCommandAsync(MaskedEmailCommand command);
    }
}