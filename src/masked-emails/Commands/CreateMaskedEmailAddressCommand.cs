using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using masked_emails.Client;
using masked_emails.Utils;
using Model;
using Refit;
using Utils;

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

            string hash = null;
            if (cmdLine.PromptPassword)
            {
                Console.WriteLine("Password:");
                var password = ConsoleUtil.GetPassword();
                hash = PasswordHelper.HashPassword(password);
                Console.WriteLine();
            }
            var request = new CreateMaskedEmailRequest
            {
                Description = cmdLine.Description ?? "",
                EnableForwarding = cmdLine.EnableForwarding,
                Name = cmdLine.Name,
                PasswordHash = hash,
            };

            try
            {
                var response = await Client.CreateAddress(request, cmdLine.EmailAddress);

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
            catch (ApiException e)
            {
                if (e.StatusCode == HttpStatusCode.Conflict)
                {
                    Console.Error.WriteLine("ERR: specified email address already exists.");
                    return;
                }

                throw;
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

            public bool EnableForwarding { get; set; }
            public string Description { get; set; }
            public string Name { get; set; }
            public string EmailAddress { get; set; }
            public bool PromptPassword { get; set; }

            public override void ParseCommandLine(string[] args)
            {
                var hasArg = false;

                AddOptions(new OptionSet {
                    {  "n|name=", v =>  {hasArg = true; Name = v; } },
                    {  "d|description=", v => {hasArg = true; Description = v; } },
                    {  "f|forward|enable-forwarding=", v => { hasArg = true; EnableForwarding = (v != null); } },
                    {  "p|password", v => PromptPassword = (v != null) },
                });

                base.ParseCommandLine(args);

                if (!hasArg && !ShowUsage)
                {
                    ShowUsage = true;
                    ShowHelp();
                }
            }
            protected override void ParseRemainingArguments(IList<string> arguments)
            {
                base.ParseRemainingArguments(arguments);
                EmailAddress = PopArgument(arguments);

                if (EmailAddress.Contains("@") && !EmailAddress.EndsWith("@maskedbox.space"))
                {
                    Console.Error.WriteLine("ERR: only email addresses on domain @maskedbox.space are currently supported.");
                    ShowUsage = true;
                }

                if (!EmailAddress.EndsWith("@maskedbox.space"))
                    EmailAddress += "@maskedbox.space";
            }

            protected override void ShowHelp()
            {
                Console.WriteLine("create a new masked email address.");
                Console.WriteLine("usage:");
                Console.WriteLine("  c | create | create-address <option>* [<email-address>]");
                Console.WriteLine("options:");
                Console.WriteLine("  -n | --name:           a name for this masked email address.");
                Console.WriteLine("  -d | --description:    an optional description for this masked email address.");
                Console.WriteLine("  -f | --forward-to:     an alternate email address to forward messages to.");
                Console.WriteLine("  -p | --password:       prompt for a password.");
                Console.WriteLine();
            }
        }
    }
}