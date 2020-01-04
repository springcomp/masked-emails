using System;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using masked_emails.Client;
using Refit;

namespace masked_emails.Commands
{
    public sealed class ToggleMaskedEmailForwardingCommand : Command
    {
        public ToggleMaskedEmailForwardingCommand(IMaskedEmailsApi client)
            : base(client)
        {
        }

        public override async Task ExecuteAsync(string[] args)
        {
            var cmdLine = CommandLine.Parse(args);
            if (cmdLine.ShowUsage)
                return;

            var address = cmdLine.EmailAddress;

            try
            {
                await Client.ToggleMaskedEmailForwarding(address);
            }
            catch (ApiException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.Error.WriteLine("ERR: no such masked email address.");
                    return;
                }

                throw;
            }
                
            var maskedEmail = await Client.GetAddress(address);

            Console.WriteLine($"{maskedEmail.EmailAddress} --> {maskedEmail.ForwardToEmailAddress}");
        }

        sealed class CommandLine : MaskedEmailAddressCommandLineParser
        {
            private CommandLine() { }
            public static CommandLine Parse(string[] args)
            {
                var cmdLine = new CommandLine();
                cmdLine.ParseCommandLine(args);
                return cmdLine;
            }

            protected override void ShowHelp()
            {
                Console.WriteLine("enable or disable a masked email forwarding.");
                Console.WriteLine("usage:");
                Console.WriteLine("  t <address>[@maskedbox.space]");
                Console.WriteLine();
            }
        }
    }
}