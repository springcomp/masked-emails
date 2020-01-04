using System;
using System.Linq;
using System.Text.RegularExpressions;
using masked_emails.Client;
using masked_emails.Commands;
using masked_emails.Utils;

namespace masked_emails
{
    public sealed class ReadEvalPrintLoop
    {
        private readonly IMaskedEmailsApi client_;
        private readonly Regex commandLineRegex_;
        private bool once_ = true;

        public ReadEvalPrintLoop(IMaskedEmailsApi client)
        {
            const string commandLinePattern = @"^(?<action>[^\ ]+)(?<args>.*)?$";

            client_ = client;
            commandLineRegex_ = new Regex(commandLinePattern, RegexOptions.Compiled | RegexOptions.Singleline);
        }

        public void Prompt(string[] options)
        {
            ShowLogo();
            ShowPrompt();

            var command = Console.ReadLine();
            if (command == "?" || command == "help")
                ShowHelp();
            if (command == "x" || command == "exit")
                return;

            var (action, args) = ParseCommandLine(command);
            if (action == Actions.Unrecognized)
            {
                Console.Error.WriteLine($"Unrecognized command '{command}'.");
            }
            else
            {
                args = args.Concat(options).ToArray();
                Command.Execute(client_, action, args);
            }

            Prompt(options);
        }

        private (Actions action, string[] args) ParseCommandLine(string command)
        {
            var match = commandLineRegex_.Match(command);
            if (!match.Success)
                return (Actions.Unrecognized, new string[] { });

            var action = match.Groups["action"].Value;
            var arguments = match.Groups["args"].Value;
            var args = CommandLineHelper.SplitArgs(arguments);

            if (!ActionsHelper.TryParseEnum(action, out var cmd))
                return (Actions.Unrecognized, new string[] { });

            return (cmd, args);
        }

        private static void ShowHelp()
        {
            Console.WriteLine("usage:");
            Console.WriteLine("  <command> [<args>*]]");
            Console.WriteLine();
            Console.WriteLine("commands:");
            Console.WriteLine(" ? | help:                       display this help screen and exit.");
            Console.WriteLine(" m | my     | my-profile:        display the user profile.");
            Console.WriteLine(" l | list   | get-addresses:     list masked email addresses.");
            Console.WriteLine(" g | get    | get-address:       display a masked email address’s detail.");
            Console.WriteLine(" c | create | create-address:    create a new masked email address.");
            Console.WriteLine(" d | delete | delete-address:    delete an existing masked email address.");
            Console.WriteLine(" t | toggle | toggle-forwarding: enable or disable a masked email forwarding.");
            Console.WriteLine(" u | update | update-address:    updates an address name or description.");
            Console.WriteLine(" x | exit:                       exit.");
            Console.WriteLine();
            Console.WriteLine("Type '<command>' or '<command> help' for detailed command usage instructions.");
            Console.WriteLine();
        }

        private static void ShowPrompt()
        {
            Console.Write("> ");
        }

        private void ShowLogo()
        {
            if (once_)
            {
                Console.WriteLine("Waiting for console input. Please, type a command.");
                Console.WriteLine("Type '?' or 'help' for usage instructions.");
                Console.WriteLine("Type 'x' or 'exit' to quit");
                once_ = false;
            }
        }
    }
}