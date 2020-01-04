using System;
using System.Net;
using System.Threading.Tasks;
using masked_emails.Client;
using Model;
using Refit;

namespace masked_emails.Commands
{
    public sealed class UpdateMaskedEmailAddressCommand : Command
    {
        public UpdateMaskedEmailAddressCommand(IMaskedEmailsApi client)
            : base(client)
        { }

        public override async Task ExecuteAsync(string[] args)
        { 
            var cmdLine = CommandLine.Parse(args);
            if (cmdLine.ShowUsage)
                return;

            var address = cmdLine.EmailAddress;

            try
            {
                var request = new UpdateMaskedEmailRequest
                {
                    Name = cmdLine.Name,
                    Description = cmdLine.Description,
                };
                await Client.UpdateAddress(address, request);
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

            Console.Out.WriteLine($"The masked email {address} has been successfully updated.");
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

            public string Description { get; set; }
            public string Name { get; set; }

            public override void ParseCommandLine(string[] args)
            { 
                var hasArg = false;

                AddOptions(new OptionSet {
                    {  "n|name=", v =>  {hasArg = true; Name = v; } },
                    {  "d|description=", v => {hasArg = true; Description = v; } },
                });

                base.ParseCommandLine(args);

                if (!hasArg)
                {
                    ShowUsage = true;
                    ShowHelp();
                }
            }

            protected override void ShowHelp()
            {
                Console.WriteLine("updates a masked email's name or description");
                Console.WriteLine("usage:");
                Console.WriteLine("  u | update | update-address <address>[@maskedbox.space] <options>");
                Console.WriteLine("options:");
                Console.WriteLine("  -n | --name:           a name for this masked email address.");
                Console.WriteLine("  -d | --description:    an optional description for this masked email address.");
                Console.WriteLine();
            }
        }
    }
}