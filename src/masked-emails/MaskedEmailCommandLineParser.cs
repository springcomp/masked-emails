using System;
using System.Collections.Generic;

namespace masked_emails
{
    public abstract class MaskedEmailCommandLineParser : CommandLineParser
    {
        protected MaskedEmailCommandLineParser()
        { 
        }

        protected override void ParseRemainingArguments(IList<string> arguments)
        {
            var argument = arguments?.Count > 0 ? arguments[0] : null;
            if (argument == "?" || argument == "h" ||  argument == "help")
            {
                PopArgument(arguments);
                ShowUsage = true;
            }
        }
    }
}