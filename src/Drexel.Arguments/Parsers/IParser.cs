using System.Collections.Generic;
using Drexel.Collections.Generic;

namespace Drexel.Arguments.Parsers
{
    internal interface IParser
    {
        void ThrowIfIllegal(
            IReadOnlySet<Argument> arguments);

        ParseResult Parse(
            IReadOnlySet<Argument> arguments,
            IReadOnlyList<string> values);
    }
}
