using System;
using System.Threading.Tasks;
using masked_emails.Client;

namespace masked_emails.Commands
{
    public sealed class GetProfileCommand : Command {
        public GetProfileCommand(IMaskedEmailsApi client)
            : base(client)
        {
        }

        public override async Task ExecuteAsync(string[] args)
        {
            var cmdLine = CommandLine.Parse(args);
            if (cmdLine.ShowUsage)
                return;

            var profile = await Client.GetProfileAsync();

            Console.WriteLine($"{profile.DisplayName}");
            Console.WriteLine($"{profile.ForwardingAddress}");

            var claims = await Client.GetClaimsAsync();

            foreach (var claim in claims)
                Console.WriteLine($"\t{claim.Type}: {claim.Value}");
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
                Console.WriteLine("display the user profile");
                Console.WriteLine("usage:");
                Console.WriteLine("  g | get-profile ");
                Console.WriteLine();
            }
        }
    }
}