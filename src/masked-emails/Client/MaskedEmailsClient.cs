using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Model;
using Refit;

namespace masked_emails.Client
{
    public sealed partial class MaskedEmailsClient : IMaskedEmailsApi
    {
        private const string Endpoint = "https://masked-emails.azurewebsites.net";

        private readonly IMaskedEmailsApi client_;

        public MaskedEmailsClient(Endpoints endpoints, NetworkCredential credentials)
        {
            var handler = new AuthenticatedHttpClientHandler(endpoints, credentials);
            client_ = RestService.For<IMaskedEmailsApi>(new HttpClient(handler) { BaseAddress = new Uri(endpoints?.Api ?? Endpoint), });
        }

        public Task<UserClaim[]> GetClaimsAsync()
        {
            return client_.GetClaimsAsync();
        }

        public Task<User> GetProfileAsync()
        {
            return client_.GetProfileAsync();
        }

        public Task<MaskedEmailWithPassword> CreateAddress(CreateMaskedEmailRequest request, string email = null)
        {
            return client_.CreateAddress(request, email);
        }

        public Task<MaskedEmail> GetAddress(string address)
        {
            return client_.GetAddress(address);
        }

        public Task ToggleMaskedEmailForwarding(string address)
        {
            return client_.ToggleMaskedEmailForwarding(address);
        }

        public Task DeleteAddress(string address)
        {
            return client_.DeleteAddress(address);
        }

        public Task<IEnumerable<MaskedEmail>> GetAddresses()
        {
            return client_.GetAddresses();
        }

        public Task UpdateAddress(string address, [Body] UpdateMaskedEmailRequest request)
        {
            return client_.UpdateAddress(address, request);
        }
    }
}