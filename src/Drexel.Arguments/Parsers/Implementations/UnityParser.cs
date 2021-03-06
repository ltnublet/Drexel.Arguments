﻿using System;
using System.Collections.Generic;
using Drexel.Collections.Generic;

namespace Drexel.Arguments.Parsers.Implementations
{
    internal sealed class UnityParser : IParser
    {
        private UnityParser()
        {
        }

        public static UnityParser Singleton { get; } = new UnityParser();

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
