using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Interop;
using Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Model;
using Utils;
using Utils.Interop;
using WebApi.Configuration;
using WebApi.Model;
using WebApi.Services.Interop;

namespace WebApi.Services
{
    public class ProfilesService : IProfilesService
    {
        private readonly IMaskedEmailsDbContext context_;
        private readonly IUniqueIdGenerator generator_;
        private readonly IMaskedEmailCommandService commands_;
        private readonly IOptions<AppSettings> settings_;

        public ProfilesService(IMaskedEmailsDbContext context, IUniqueIdGenerator generator, IMaskedEmailCommandService commands, IOptions<AppSettings> settings)
        {
            context_ = context;
            generator_ = generator;
            commands_ = commands;
            settings_ = settings;
        }

        public virtual async Task<IEnumerable<User>> GetProfiles()
        {
            var records = await QueryProfileEntities().ToListAsync();
            var profiles = records.Select(r => r.ToModel());

            return profiles;
        }

        public virtual async Task<User> GetProfile(string userId)
        {
            var record = await GetProfileEntity(userId);
            if (record == null)
                throw Error.NoSuchProfile(userId);

            var profile = record.ToModel();

            return profile;
        }

        public virtual async Task<User> UpdateProfile(string userId, string displayName, string forwardingAddress)
        {
            var record = await GetProfileEntityGraphForUpdate(userId);
            if (record == null)
                throw Error.NoSuchProfile(userId);

            record.DisplayName = displayName;
            record.ForwardingAddress = forwardingAddress;

            await context_.SaveChangesAsync();

            var profile = record.ToModel();

            return profile;
        }

        public async Task<IEnumerable<MaskedEmail>> GetMaskedEmails(string userId)
        {
            var record = await GetProfileEntityGraph(userId);
            var collection = record.Addresses.Select(a => a.ToModel());

            return collection;
        }

        public async Task<MaskedEmail> GetMaskedEmail(string userId, string email)
        {
            var record = await GetProfileEntityGraph(userId);
            if (record == null)
                throw Error.NoSuchProfile(userId);

            var address = record.Addresses.SingleOrDefault(a => a.EmailAddress == email);
            if (address == null)
                throw Error.NoSuchMaskedEmailAddress(email);

            return address.ToModel();
        }

        public async Task<MaskedEmail> ToggleMaskedEmailForwarding(string userId, string email)
        {
            var address = await GetAddressEntityForUpdate(userId, email);

            await (
                    address.EnableForwarding
                        ? commands_.DisableMaskedEmailAsync(address.EmailAddress)
                        : commands_.EnableMaskedEmailAsync(address.EmailAddress)
                )
                ;

            address.EnableForwarding = !address.EnableForwarding;
            await context_.SaveChangesAsync();

            return address.ToModel();
        }

        public async Task<MaskedEmailWithPassword> CreateMaskedEmail(string userId, string name, string passwordHash, string description, bool enableForwarding)
        {
            var record = await GetProfileEntityGraphForUpdate(userId);
            if (record == null)
                throw Error.NoSuchProfile(userId);

            string clearTextPassword = null;

            var emailAddress = await MakeUniqueAddressAsync();
            if (String.IsNullOrEmpty(passwordHash))
            {
                clearTextPassword = MakePassword();
                passwordHash = MakePassword();
            }

            await commands_.CreateMaskedEmailAsync(
                emailAddress
                , record.ForwardingAddress
                , passwordHash
            );

            var address = new Address
            {
                Name = name,
                Description = description,
                EmailAddress = emailAddress,
                EnableForwarding = enableForwarding,
                CreatedUtc = DateTime.UtcNow,
            };

            record.Addresses.Add(address);
            await context_.SaveChangesAsync();

            // TODO: Received ?

            var result = MaskedEmailWithPassword.Clone(address.ToModel());
            result.Password = clearTextPassword;

            return result;
        }

        public async Task UpdateMaskedEmail(string userId, string email, string name, string description)
        { 
            var address = await GetAddressEntityForUpdate(userId, email);

            if (!string.IsNullOrEmpty(name) && address.Name != name)
                address.Name = name;

            if (address.Description != description)
                address.Description = description;

            await context_.SaveChangesAsync();
        }

        public async Task DeleteMaskedEmail(string userId, string email)
        {
            var address = await GetAddressEntityForUpdate(userId, email);

            await commands_.RemoveMaskedEmailAsync(address.EmailAddress);

            context_.Addresses.Remove(address);
            await context_.SaveChangesAsync();
        }

        #region Implementation

        private IQueryable<Profile> QueryProfileEntities()
        {
            return context_
                .Users
                ;
        }

        private async Task<Profile> GetProfileEntity(string userId)
        {
            return
                await QueryProfileEntities()
                    .AsNoTracking()
                    .SingleOrDefaultAsync(p => p.Id == userId)
                ;
        }

        private async Task<Profile> GetProfileEntityGraph(string userId)
        {
            return
                await QueryProfileEntities()
                    .Include(p => p.Addresses)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(p => p.Id == userId)
                ;
        }

        private async Task<Profile> GetProfileEntityGraphForUpdate(string userId)
        {
            return
                await QueryProfileEntities()
                    .Include(e => e.Addresses)
                    .SingleOrDefaultAsync(p => p.Id == userId)
                ;
        }

        private async Task<Address> GetAddressEntityForUpdate(string userId, string email)
        {
            var record = await GetProfileEntityGraphForUpdate(userId);
            if (record == null)
                throw Error.NoSuchProfile(userId);

            var address = record.Addresses.SingleOrDefault(a => a.EmailAddress == email);
            if (address == null)
                throw Error.NoSuchMaskedEmailAddress(email);

            return address;
        }

        #endregion

        private string MakePassword()
        {
            return PasswordHelper.GeneratePassword(12);
        }
        private async Task<string> MakeUniqueAddressAsync()
        {
            var domain = settings_.Value.DomainName;
            var identifier = await generator_.GenerateAsync();

            return $"{identifier}@{domain}";
        }
    }
}