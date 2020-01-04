using System.Collections.Generic;

namespace masked_emails
{
    public abstract class MaskedEmailCommandLineParser : CommandLineParser
    {
        protected MaskedEmailCommandLineParser()
            : base(new OptionSet { })
        { }

        public override void ParseCommandLine(string[] args)
        {
            base.ParseCommandLine(args);
            if (ShowUsage)
                ShowHelp();
        }

        protected override void ParseRemainingArguments(IList<string> arguments)
        {
            var argument = PopArgument(arguments);
            if (argument == "?" || argument == "help")
                ShowUsage = true;
        }
    }
}