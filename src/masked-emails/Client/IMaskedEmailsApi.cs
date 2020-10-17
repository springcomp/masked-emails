using System.Collections.Generic;
using System.Threading.Tasks;
using Model;
using Refit;

namespace masked_emails.Client
{
    public interface IMaskedEmailsApi
    {
        [Get("/api/claims")]
        [Headers("Authorization: Bearer")]
        Task<UserClaim[]> GetClaimsAsync();

        [Get("/profiles/my")]
        [Headers("Authorization: Bearer")]
        Task<User> GetProfileAsync();


        [Post("/profiles/my/addresses")]
        [Headers("Authorization: Bearer")]
        Task<MaskedEmailWithPassword> CreateAddress([Body] CreateMaskedEmailRequest request);

        [Get("/profiles/my/addresses")]
        [Headers("Authorization: Bearer")]
        Task<IEnumerable<MaskedEmail>> GetAddresses();

        [Get("/profiles/my/addresses/{address}")]
        [Headers("Authorization: Bearer")]
        Task<MaskedEmail> GetAddress(string address);

        [Patch("/profiles/my/addresses/{address}")]
        [Headers("Authorization: Bearer")]
        Task UpdateAddress(string address, [Body] UpdateMaskedEmailRequest request);

        [Patch("/profiles/my/addresses/{address}/enableForwarding")]
        [Headers("Authorization: Bearer")]
        Task ToggleMaskedEmailForwarding(string address);

        [Delete("/profiles/my/addresses/{address}")]
        [Headers("Authorization: Bearer")]
        Task DeleteAddress(string address);
    }
}