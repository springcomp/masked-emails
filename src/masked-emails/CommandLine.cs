using System;
using System.Collections.Generic;
using System.Linq;
using masked_emails.Utils;

namespace masked_emails
{
    public sealed class CommandLine : CommandLineParser
    {
        private CommandLine() { }
        public static CommandLine Parse(string[] args)
        {
            var cmdLine = new CommandLine();
            cmdLine.AddOptions(new OptionSet
            {
                {"u|user|username=", v => cmdLine.Username = v},
                {"p|pwd|password=", v => cmdLine.Password = v},
            });

            cmdLine.ParseCommandLine(args);
            return cmdLine;
        }

        protected override void ParseRemainingArguments(IList<string> remainders)
        {
            var command = PopArgument(remainders);
            if (!String.IsNullOrEmpty(command))
            {
                if (!ActionsHelper.TryParseEnum(command, out var cmd))
                {
                    Exit($"Unrecognized command {command}.");
                }

                Action = cmd;
            }

            Args = remainders.ToArray();
        }
        protected override void ShowHelp()
        {
            Console.WriteLine("masked-emails : create or update masked email addresses.");
            Console.WriteLine("2019 - https://github.com/springcomp");
            Console.WriteLine("usage:");
            Console.WriteLine("  masked-emails [<option>] [<command> [<args>*]]");
            Console.WriteLine();
            Console.WriteLine("run without options to show an interactive prompt (REPL).");
            Console.WriteLine();
            Console.WriteLine("options:");
            Console.WriteLine("  -? | -h    | --help:           display this help screen and exit.");
            Console.WriteLine("  -u | -user | --username:       specify the username.");
            Console.WriteLine("  -p | -pwd  | --password:       specify the password.");
            Console.WriteLine();
            Console.WriteLine("commands:");
            Console.WriteLine(" m | me     | my-profile:        display the user profile.");
            Console.WriteLine(" l | list   | get-addresses:     list masked email addresses.");
            Console.WriteLine(" g | get    | get-address:       display a masked email address’s detail.");
            Console.WriteLine(" c | create | create-address:    create a new masked email address.");
            Console.WriteLine(" d | delete | delete-address:    delete an existing masked email address.");
            Console.WriteLine(" t | toggle | toggle-forwarding: enable or disable a masked email forwarding.");
            Console.WriteLine();
            Console.WriteLine("all commands share the following arg:");
            Console.WriteLine("  -? | -h | --help:              display the command help and exit.");
            Console.WriteLine();
            Console.WriteLine("examples:");
            Console.WriteLine("  masked-emails -u <usr> -p <pwd> t <address>[@maskedbox.space]");
            Console.WriteLine("  masked-emails -u <usr> -p <pwd> c <address>[@maskedbox.space] --forward-to <other@domain.tld>");
            Console.WriteLine();
            Console.WriteLine("run without options to show an interactive prompt.");
            Console.WriteLine("this shows a read-eval-print-loop shell:");
            Console.WriteLine();
            Console.WriteLine("  masked-emails");
            Console.WriteLine("  > t address@maskedbox.space");
            Console.WriteLine("  > c address@maskedbox.space --forward-to <other@domain.tld>");
            Console.WriteLine();

            Environment.Exit(0);
        }

        protected override void OnParseError(OptionException e)
        {
            Exit("Syntax error.");
        }

        private static void Exit(string message)
        {
            Console.Error.WriteLine(message);
            Environment.Exit(42);
        }

        public string Username { get; private set; }
        public string Password { get; private set; }
        public Actions Action { get; private set; }
        public string[] Args { get; private set; }
    }
}