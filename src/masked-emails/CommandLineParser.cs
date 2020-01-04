using System.Collections.Generic;

namespace masked_emails
{
    public abstract class CommandLineParser
    {
        private readonly OptionSet options_;

        protected CommandLineParser()
        {
            options_ = new OptionSet
            {
                {"h|help", v => ShowUsage = v != null},
                {"?", v => ShowUsage = v != null},
            };
        }

        protected OptionSet Options => options_;

        protected CommandLineParser(OptionSet options)
        {
            options_ = options;
        }

        public void AddOptions(OptionSet options)
        {
            foreach (var option in options)
                options_.Add(option);
        }

        public virtual void ParseCommandLine(string[] args)
        {
            try
            {
                var remainders = Options.Parse(args);
                if (ShowUsage)
                    ShowHelp();

                ParseRemainingArguments(remainders);
            }
            catch (OptionException e)
            {
                OnParseError(e);
            }
        }

        public bool ShowUsage { get; protected set; }

        protected abstract void ShowHelp();
        protected virtual void ParseRemainingArguments(IList<string> arguments) { }
        protected virtual void OnParseError(OptionException e) { }

        protected static string PopArgument(IList<string> collection)
        {
            if (collection.Count > 0)
            {
                var item = collection[0];
                collection.RemoveAt(0);
                return item;
            }

            return null;
        }
    }
}