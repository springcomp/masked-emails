using System;
using System.Net;
using System.Threading.Tasks;
using masked_emails.Client;
using Refit;

namespace masked_emails.Commands
{
    public sealed class DeleteMaskedEmailCommand : Command
    { 
        public DeleteMaskedEmailCommand(IMaskedEmailsApi client)
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
                await Client.DeleteAddress(address);
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

            Console.Out.WriteLine($"The masked email {address} has been successfully deleted.");
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
                Console.WriteLine("delete an existing masked email address.");
                Console.WriteLine("usage:");
                Console.WriteLine("  d | delete | delete-address <address>[@maskedbox.space]");
                Console.WriteLine();
            }
        }
    }
}