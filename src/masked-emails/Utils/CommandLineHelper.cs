using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace masked_emails.Utils
{
    public sealed class CommandLineHelper
    {
        private const string ArgsPattern = "^([^\"\\\\ ]+)|\"(?:[^\"\\\\]|\\\\.)*\"";
        private static readonly Regex ArgsRegex = new Regex(ArgsPattern, RegexOptions.Singleline | RegexOptions.Compiled);

        public static string[] SplitArgs(string args)
        {
            var result = new List<string>();

            var text = args.Trim();
            while (text.Length > 0)
            {
                var match = ArgsRegex.Match(text);
                System.Diagnostics.Debug.Assert(match.Success);

                result.Add(match.Value);

                text = text.Substring(match.Length).Trim();
            }

            return result.ToArray();
        }
    }
}