using MaskedEmails.Inbox.Model;
using Refit;
using System.Threading.Tasks;

namespace MaskedEmails.Inbox.Interop
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
