using System;
using System.Collections.Generic;
using Drexel.Collections.Generic;

namespace Drexel.Arguments.Parsers
{
    internal sealed class MsBuildParser : IParser
    {
        private MsBuildParser()
        {
        }

        public static MsBuildParser Singleton { get; } = new MsBuildParser();

        public ParseResult Parse(IReadOnlySet<Argument> arguments, IReadOnlyList<string> values)
        {
            throw new NotImplementedException();
        }

        public void ThrowIfIllegal(IReadOnlySet<Argument> arguments)
        {
            throw new NotImplementedException();
        }
    }
}
