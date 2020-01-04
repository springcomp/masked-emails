using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using masked_emails.Client;
using Model;

namespace masked_emails.Commands
{
    public sealed class CreateMaskedEmailAddressCommand : Command
    {
        public CreateMaskedEmailAddressCommand(IMaskedEmailsApi client)
            : base(client)
        {
        }

        public override async Task ExecuteAsync(string[] args)
        {
            var cmdLine = CommandLine.Parse(args);
            if (cmdLine.ShowUsage)
                return;

            var request = new MaskedEmailRequest {
                Description = cmdLine.Description ?? "",
                EnableForwarding = true,
                Name = cmdLine.Name,
                PasswordHash = cmdLine.PasswordHash,
            };

            var response = await Client.CreateAddress(request);

            Console.Out.WriteLine($"Masked email address successfully created:");
            Console.Out.WriteLine($"{response.EmailAddress} --> {response.ForwardToEmailAddress}");
            if (!String.IsNullOrEmpty(response.Password))
            {
                Console.Out.WriteLine($"WRN: ==== PASSWORD: {response.Password} ====");
                Console.Out.WriteLine($"WRN: The password shown here cannot be retrieved.");
                Console.Out.WriteLine($"WRN: If you want to connect to the mailbox, please");
                Console.Out.WriteLine($"WRN: make sure to copy the password to a safe place.");
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

            public string AlternateAddress { get; set; }
            public string Description { get; set; }
            public string Name { get; set; }
            public string PasswordHash { get; set; }

            public override void ParseCommandLine(string[] args)
            {
                var hasArg = false;

                AddOptions(new OptionSet {
                    {  "n|name=", v =>  {hasArg = true; Name = v; } },
                    {  "d|description=", v => {hasArg = true; Description = v; } },
                    {  "f|forward|forward-to=", v => { hasArg = true; AlternateAddress = v; } },
                    {  "p|password-hash=", v => PasswordHash = v },
                });

                base.ParseCommandLine(args);

                if (!hasArg)
                {
                    ShowUsage = true;
                    ShowHelp();
                }
            }
            protected override void ParseRemainingArguments(IList<string> arguments)
            {
                base.ParseRemainingArguments(arguments);
                var argument = PopArgument(arguments);
                if (String.IsNullOrEmpty(AlternateAddress))
                {
                    Console.Error.WriteLine("ERR: please specify the required -f or --forward-to option.");
                }
            }

            protected override void ShowHelp()
            {
                Console.WriteLine("create a new masked email address.");
                Console.WriteLine("usage:");
                Console.WriteLine("  c | create | create-address <option>*");
                Console.WriteLine("options:");
                Console.WriteLine("  -n | --name:           a name for this masked email address.");
                Console.WriteLine("  -d | --description:    an optional description for this masked email address.");
                Console.WriteLine("  -f | --forward-to:     an alternate email address to forward messages to.");
                Console.WriteLine();
            }
        }
    }
}