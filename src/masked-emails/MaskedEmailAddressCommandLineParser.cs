﻿using System;
using System.Collections.Generic;

namespace masked_emails
{
    public abstract class MaskedEmailAddressCommandLineParser : MaskedEmailCommandLineParser
    {
        public string EmailAddress { get; protected set; }

        protected override void ParseRemainingArguments(IList<string> arguments)
        {
            var argument = PopArgument(arguments);
            if (argument == null || argument == "?" || argument == "help")
                ShowUsage = true;
            else
                EmailAddress = argument;

            if (String.IsNullOrEmpty(EmailAddress))
                ShowUsage = true;
        }
    }
}