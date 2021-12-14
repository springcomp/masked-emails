using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace MaskedEmails.Services.Interop
{
    public interface IProfilesService
    {
        Task<IEnumerable<User>> GetProfiles();
        Task<User> GetProfile(string userId);
        Task<User> UpdateProfile(string userId, string displayName, string forwardingAddress);

        Task<IEnumerable<MaskedEmail>> GetMaskedEmails(string userId);
        Task<GetMaskedEmailPageResponse> GetMaskedEmails(string userId, int top, string cursor, string sort_by, string search);
        Task<MaskedEmail> GetMaskedEmail(string userId, string email);

        Task<MaskedEmail> ToggleMaskedEmailForwarding(string userId, string email);
        Task<MaskedEmailWithPassword> CreateMaskedEmail(string userId, string name, string passwordHash, string description = null, bool enableForwarding = true);
        Task<MaskedEmailWithPassword> CreateMaskedEmail(string userId, string name, string email, string passwordHash, string description = null, bool enableForwarding = true);
        Task UpdateMaskedEmail(string userId, string email, string name, string description, string passwordHash = null);
        Task DeleteMaskedEmail(string userId, string email);
    }
}