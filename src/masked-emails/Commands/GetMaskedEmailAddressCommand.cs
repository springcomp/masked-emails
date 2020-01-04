using System;
using System.Net;
using System.Threading.Tasks;
using masked_emails.Client;
using Refit;

namespace masked_emails.Commands
{
    public sealed class GetMaskedEmailAddressCommand : Command
    { 
        public GetMaskedEmailAddressCommand(IMaskedEmailsApi client)
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
                var maskedEmail = await Client.GetAddress(address);

                Console.WriteLine($"{maskedEmail.Name}");
                if (!String.IsNullOrEmpty(maskedEmail.Description))
                    Console.WriteLine($"{maskedEmail.Description}");
                Console.WriteLine($"{maskedEmail.EmailAddress} --> {maskedEmail.ForwardToEmailAddress}");
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
                Console.WriteLine("get a masked emails address's details.");
                Console.WriteLine("usage:");
                Console.WriteLine("  g | get | get-address <address>[@maskedbox.space]");
                Console.WriteLine();
            }
        }
    }
}