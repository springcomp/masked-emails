using System;
using System.Linq;
using System.Threading.Tasks;
using masked_emails.Client;

namespace masked_emails.Commands
{
    public sealed class GetMaskedEmailAddressesCommand : Command {
        public GetMaskedEmailAddressesCommand(IMaskedEmailsApi client)
            : base(client)
        {
        }

        public override async Task ExecuteAsync(string[] args)
        {
            var cmdLine = CommandLine.Parse(args);
            if (cmdLine.ShowUsage)
                return;

            var collection = (await Client.GetAddresses())
                    .ToArray()
                ;
            for (var index = 0; index < collection.Length; index++)
            {
                var maskedEmail = collection[index];
                Console.WriteLine($"{index + 1,2} - {maskedEmail.Name}");
                if (!String.IsNullOrEmpty(maskedEmail.Description))
                    Console.WriteLine($"     {maskedEmail.Description}");
                Console.WriteLine($"     {maskedEmail.EmailAddress} --> {maskedEmail.ForwardToEmailAddress}");
            }
        }
        sealed class CommandLine : MaskedEmailCommandLineParser
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
                Console.WriteLine("list masked email addresses.");
                Console.WriteLine("usage:");
                Console.WriteLine("  l | list | get-addresses | get-masked-email-addresses");
                Console.WriteLine();
            }
        }
    }
}