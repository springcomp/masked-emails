using System;
using System.Threading.Tasks;
using masked_emails.Client;

namespace masked_emails.Commands
{
    public sealed class GetProfileCommand : Command
    {
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

            if (cmdLine.ShowClaims)
            {
                var claims = await Client.GetClaimsAsync();

                Console.WriteLine();
                Console.WriteLine("JWT claims:");

                foreach (var claim in claims)
                    Console.WriteLine($"  {claim.Type}: {claim.Value}");
            }
        }

        sealed class CommandLine : MaskedEmailCommandLineParser
        {
            private CommandLine() : base() { }
            public static CommandLine Parse(string[] args)
            {
                var cmdLine = new CommandLine();
                cmdLine.ParseCommandLine(args);
                return cmdLine;
            }

            public bool ShowClaims { get; set; }
            public override void ParseCommandLine(string[] args)
            {
                AddOptions(new OptionSet {
                    {  "c|claims|show-claims", v =>  ShowClaims = (v != null) },
                });

                base.ParseCommandLine(args);
            }

            protected override void ShowHelp()
            {
                Console.WriteLine("display the user profile");
                Console.WriteLine("usage:");
                Console.WriteLine("  g | get-profile ");
                Console.WriteLine("options:");
                Console.WriteLine("  -c | --claims | --show-claims      show claims from the JWT token");
                Console.WriteLine();
            }
        }
    }
}