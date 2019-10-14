using System;
using System.Collections.Generic;
using Drexel.Collections.Generic;

namespace Drexel.Arguments.Parsers
{
    internal sealed class PosixParser : IParser
    {
        private PosixParser()
        {
        }

        public static PosixParser Singleton { get; } = new PosixParser();

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
