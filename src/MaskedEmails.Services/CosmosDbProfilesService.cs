using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CosmosDb.Model.Interop;
using Microsoft.Extensions.Options;
using Model;
using Utils;
using Utils.Interop;
using WebApi.Configuration;
using WebApi.Model;
using WebApi.Services.Interop;

namespace WebApi.Services
{
    using Profile = CosmosDb.Model.Profile;
    using Address = CosmosDb.Model.Address;

    public class CosmosDbProfilesService : IProfilesService
    {
        private readonly ICosmosDbContext context_;
        private readonly IUniqueIdGenerator generator_;
        private readonly IMaskedEmailCommandService commands_;
        private readonly IOptions<AppSettings> settings_;

        private const string ISO8601_DATE_TIME_FORMAT = "yyyy-MM-ddTHH:mm:ss.fffffff";

        public CosmosDbProfilesService(ICosmosDbContext context, IUniqueIdGenerator generator, IMaskedEmailCommandService commands, IOptions<AppSettings> settings)
        {
            context_ = context;
            generator_ = generator;
            commands_ = commands;
            settings_ = settings;
        }

        public virtual async Task<IEnumerable<User>> GetProfiles()
        {
            // TODO: return IAsyncEnumerable when SQLite is no longer necessary.
            // TODO: for now we keep this ugly code below 😥

            var profiles = new List<User>();

            var records = context_.QueryProfiles();
            await foreach (var page in records)
                foreach (var record in page)
                    profiles.Add(record.ToModel());

            return profiles;
        }

        public virtual async Task<User> GetProfile(string userId)
        {
            var record = await context_.GetProfile(userId);
            if (record == null)
                throw Error.NoSuchProfile(userId);

            var profile = record.ToModel();

            return profile;
        }

        public virtual async Task<User> UpdateProfile(string userId, string displayName, string forwardingAddress)
        {
            var record = await context_.GetProfile(userId);
            if (record == null)
                throw Error.NoSuchProfile(userId);

            record.DisplayName = displayName;
            record.ForwardingAddress = forwardingAddress;

            await context_.UpdateProfile(record);

            var profile = record.ToModel();

            return profile;
        }

        public async Task<IEnumerable<MaskedEmail>> GetMaskedEmails(string userId)
        {
            var record = await context_.GetProfile(userId);
            if (record == null)
                throw Error.NoSuchProfile(userId);

            var collection = record
                .Addresses
                .Select(a => a.ToModel())
                ;

            return collection;
        }

        public async Task<GetMaskedEmailPageResponse> GetMaskedEmails(string userId, int top, string cursor, string sort_by, string contains)
        {
            var record = await context_.GetProfile(userId);
            if (record == null)
                throw Error.NoSuchProfile(userId);

            var response = new GetMaskedEmailPageResponse
            {
                Total = record.Addresses.Count,
            };

            var descending = (sort_by.EndsWith("-desc"));
            if (descending)
            {
                sort_by = sort_by.Substring(0, sort_by.Length - "-desc".Length);
            }
            var when = ParseCursor(cursor, descending);

            var selectPage = MakePageSelector(when, descending);
            var searchExpression = MakeSearchSelector(contains);

            var collection = record.Addresses
                .Where(selectPage)
                .Where(searchExpression)
                .OrderBy(sort_by, descending)
                ;

            var array = collection
                .Select(a => a.ToModel())
                .Take(top)
                .ToArray()
                ;

            response.Addresses = array;
            response.Count = array.Length;

            if (array.Length > 0)
                response.Cursor = MakeCursor(array[^1].CreatedUtc);

            return response;
        }

        public async Task<MaskedEmail> GetMaskedEmail(string userId, string email)
        {
            var record = await context_.GetProfile(userId);
            if (record == null)
                throw Error.NoSuchProfile(userId);

            var address = record.Addresses.SingleOrDefault(a => a.EmailAddress == email);
            if (address == null)
                throw Error.NoSuchMaskedEmailAddress(email);

            return address.ToModel();
        }

        public async Task<MaskedEmail> ToggleMaskedEmailForwarding(string userId, string email)
        {
            var record = await context_.GetProfile(userId);
            if (record == null)
                throw Error.NoSuchProfile(userId);

            var address = record.Addresses.SingleOrDefault(a => a.EmailAddress == email);
            if (address == null)
                throw Error.NoSuchMaskedEmailAddress(email);

            address.EnableForwarding = !address.EnableForwarding;
            await context_.UpdateProfile(record);

            // update mailbox forwarding at the mail server

            await (
                    address.EnableForwarding
                        ? commands_.EnableMaskedEmailAsync(address.EmailAddress)
                        : commands_.DisableMaskedEmailAsync(address.EmailAddress)
                )
                ;

            return address.ToModel();
        }

        public async Task<MaskedEmailWithPassword> CreateMaskedEmail(string userId, string name, string passwordHash, string description, bool enableForwarding)
        {
            return await CreateMaskedEmail(
                userId,
                name,
                await MakeUniqueAddressAsync(),
                passwordHash,
                description,
                enableForwarding
                );
        }

        public async Task<MaskedEmailWithPassword> CreateMaskedEmail(string userId, string name, string email, string passwordHash, string description, bool enableForwarding)
        {
            var record = await context_.GetProfile(userId);
            if (record == null)
                throw Error.NoSuchProfile(userId);

            string clearTextPassword = null;

            var emailAddress = email;
            var exists = record.Addresses.ToList().SingleOrDefault(a => String.Compare(a.EmailAddress, emailAddress, true) == 0);
            if (exists != null)
                throw new ArgumentException("The specified email address already exists.");

            if (String.IsNullOrEmpty(passwordHash))
            {
                clearTextPassword = MakePassword();
                passwordHash = HashPassword(clearTextPassword);
            }

            var address = new Address
            {
                Name = name,
                Description = description,
                EmailAddress = emailAddress,
                EnableForwarding = enableForwarding,
                CreatedUtc = DateTime.UtcNow,
            };

            record.Addresses.Add(address);
            await context_.UpdateProfile(record);

            var result = MaskedEmailWithPassword.Clone(address.ToModel());
            result.Password = clearTextPassword;

            // create mailbox at the mail server

            await commands_.CreateMaskedEmailAsync(
                emailAddress
                , record.ForwardingAddress
                , passwordHash
            );

            return result;
        }

        public async Task UpdateMaskedEmail(string userId, string email, string name, string description, string passwordHash = null)
        {
            var record = await context_.GetProfile(userId);
            if (record == null)
                throw Error.NoSuchProfile(userId);

            var address = record.Addresses.SingleOrDefault(a => a.EmailAddress == email);
            if (address == null)
                throw Error.NoSuchMaskedEmailAddress(email);

            if (!string.IsNullOrEmpty(name) && address.Name != name)
                address.Name = name;

            if (address.Description != description)
                address.Description = description;

            await context_.UpdateProfile(record);

            // if a password hash is specified
            // change the password on the mail server

            if (!string.IsNullOrWhiteSpace(passwordHash))
            {
                await commands_.ChangeMaskedEmailPassword(
                    address.EmailAddress, passwordHash
                    );
            }
        }

        public async Task DeleteMaskedEmail(string userId, string email)
        {
            var record = await context_.GetProfile(userId);
            if (record == null)
                throw Error.NoSuchProfile(userId);

            record.Addresses.RemoveAll(a => a.EmailAddress == email);

            await context_.UpdateProfile(record);

            // remove the mailbox from the mail server

            await commands_.RemoveMaskedEmailAsync(record.EmailAddress);
        }

        #region Implementation

        private static DateTime ParseCursor(string cursor, bool descending)
        {
            // cursor is an ISO-8601 date

            DateTime when = DateTime.MinValue;
            if (String.IsNullOrEmpty(cursor))
            {
                if (descending)
                    when = DateTime.MaxValue;
            }
            else
            {
                if (!DateTime.TryParseExact(cursor, ISO8601_DATE_TIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out when))
                    throw new ArgumentException("Invalid cursor syntax.");
            }

            return when;
        }

        private string MakeCursor(DateTime createdUtc)
        {
            return createdUtc.ToString(ISO8601_DATE_TIME_FORMAT);
        }

        private static Func<Address, bool> MakeSearchSelector(string contains)
        {
            Func<Address, bool> where =
                a => true;

            if (string.IsNullOrEmpty(contains))
                return where;

            where = a =>
                   a.Name.Contains(contains, StringComparison.InvariantCultureIgnoreCase)
                || a.Description.Contains(contains, StringComparison.InvariantCultureIgnoreCase)
                || a.EmailAddress.Contains(contains, StringComparison.InvariantCultureIgnoreCase)
                ;

            return where;
        }
        private static Func<Address, bool> MakePageSelector(DateTime when, bool descending)
        {
            Func<Address, bool> where =
                a => a.CreatedUtc > when;
            if (descending)
                where = a => a.CreatedUtc < when;

            return where;
        }

        private string MakePassword()
        {
            return PasswordHelper.GeneratePassword(settings_.Value.PasswordLength);
        }
        private string HashPassword(string clearText)
        {
            return PasswordHelper.HashPassword(clearText);
        }
        private async Task<string> MakeUniqueAddressAsync()
        {
            var domain = settings_.Value.DomainName;
            var identifier = await generator_.GenerateAsync();

            return $"{identifier}@{domain}";
        }

        #endregion
    }
}