using System;
using System.Threading.Tasks;
using masked_emails.Client;

namespace masked_emails.Commands
{
    public abstract class Command
    {
        protected Command(IMaskedEmailsApi client)
        {
            Client = client;
        }

        protected IMaskedEmailsApi Client { get; }

        public virtual void Execute(string[] args)
        {
            ExecuteAsync(args)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult()
                ;
        }
        public abstract Task ExecuteAsync(string[] args);
        public static void Execute(IMaskedEmailsApi client, Actions action, string[] args)
        {
            Create(client, action).Execute(args);
        }
        public static Task ExecuteAsync(IMaskedEmailsApi client, Actions action, string[] args)
        {
            return Create(client, action).ExecuteAsync(args);
        }
        private static Command Create(IMaskedEmailsApi client, Actions action)
        {
            switch (action)
            {
                case Actions.GetProfile:
                    return new GetProfileCommand(client);

                case Actions.CreateMaskedEmail:
                    return new CreateMaskedEmailAddressCommand(client);

                case Actions.UpdateMaskedEmail:
                    return new UpdateMaskedEmailAddressCommand(client);

                case Actions.DeleteMaskedEmail:
                    return new DeleteMaskedEmailCommand(client);

                case Actions.GetMaskedEmailAddresses:
                    return new GetMaskedEmailAddressesCommand(client);

                case Actions.GetMaskedEmailAddress:
                    return new GetMaskedEmailAddressCommand(client);

                case Actions.ToggleMaskedEmailAddressForwarding:
                    return new ToggleMaskedEmailForwardingCommand(client);
            }

            System.Diagnostics.Debug.Assert(false);
            throw new NotSupportedException();
        }
    }
}