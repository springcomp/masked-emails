using System.Threading.Tasks;
using Utils;
using WebApi.Model.QueueRequests;
using WebApi.Services.Interop;

namespace WebApi.Services
{
    public static class MaskedEmailCommandServiceExtensions
    {
        public static async Task CreateMaskedEmailAsync(this IMaskedEmailCommandService service
            , string address
            , string forwardTo
            , string passwordHash
            , bool forwardingEnabled = true)
        {
            var addCommand = new CreateMaskedEmailCommand
            {

                Address = address,
                AlternateAddress = forwardTo,
                PasswordHash = passwordHash,
            };
            await service.QueueCommandAsync(addCommand);

            if (!forwardingEnabled)
                await service.DisableMaskedEmailAsync(address);
        }

        public static async Task EnableMaskedEmailAsync(this IMaskedEmailCommandService service
            , string address)
        { 
            var addCommand = new EnableMaskedEmailCommand
            {
                Address = address,
            };
            await service.QueueCommandAsync(addCommand);
        }
        public static async Task DisableMaskedEmailAsync(this IMaskedEmailCommandService service
            , string address)
        { 
            var addCommand = new DisableMaskedEmailCommand
            {
                Address = address,
            };
            await service.QueueCommandAsync(addCommand);
        }
        public static async Task RemoveMaskedEmailAsync(this IMaskedEmailCommandService service
            , string address)
        { 
            var addCommand = new RemoveMaskedEmailCommand
            {
                Address = address,
            };
            await service.QueueCommandAsync(addCommand);
        }
        public static async Task ChangeMaskedEmailPassword(this IMaskedEmailCommandService service
            , string address
            , string passwordHash)
        {
            var changePasswordCommand = new ChangeMaskedEmailPasswordCommand
            {
                Address = address,
                PasswordHash = passwordHash,
            };
            await service.QueueCommandAsync(changePasswordCommand);
        }
    }
}