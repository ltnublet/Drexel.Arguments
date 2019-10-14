using System;
using System.Collections.Generic;
using Drexel.Collections.Generic;

namespace Drexel.Arguments.Parsers
{
    internal sealed class DosParser : IParser
    {
        private DosParser()
        {
        }

        public static DosParser Singleton { get; } = new DosParser();

        public void ThrowIfIllegal(IReadOnlySet<Argument> arguments)
        {
            HashSet<char> characters = new HashSet<char>();
            foreach (Argument argument in arguments)
            {
                foreach (string shortName in argument.ShortNames)
                {
                    if (shortName.Length > 1)
                    {
                        throw new ArgumentException(
                            "DOS-style parsing requires that short names be only one character long.");
                    }

                    char character = shortName[0];
                    if (character == '/')
                    {
                        throw new ArgumentException(
                            "DOS-style parsing means names cannot start with '/'.");
                    }

                    if (!characters.Add(character))
                    {
                        throw new ArgumentException(
                            "DOS-style parsing requires that short names have a unique first character.");
                    }
                }
            }
        }

        public ParseResult Parse(
            IReadOnlySet<Argument> arguments,
            IReadOnlyList<string> values)
        {
            throw new NotImplementedException();
        }
    }
}
