using Refit;
using System.Threading.Tasks;

namespace InboxApi.Model.Interop
{
    public interface IInboxApi
    {
        [Post("/emails")]
        [Headers("Authorization: Bearer")]
        Task<InboxMessageSpec[]> GetMessages([Body] string[] inboxes);

        [Post("/emails/body")]
        [Headers("Authorization: Bearer")]
        Task<InboxMessage> GetMessageBody([Body] string location);
    }
}
