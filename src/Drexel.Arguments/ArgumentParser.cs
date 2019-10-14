﻿using System;
using System.Collections.Generic;
using Drexel.Arguments.Parsers;
using Drexel.Collections.Generic;

namespace Drexel.Arguments
{
    public sealed class ArgumentParser
    {
        private readonly IParser parser;

        public ArgumentParser(ParseStyle style, IReadOnlySet<Argument> arguments)
        {
            this.Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
            this.Style = style;

            this.parser = style switch
            {
                ParseStyle.DOS => DosParser.Singleton,
                ParseStyle.GNU => GnuParser.Singleton,
                ParseStyle.MSBuild => MsBuildParser.Singleton,
                ParseStyle.Posix => PosixParser.Singleton,
                ParseStyle.Windows => WindowsParser.Singleton,
                _ => throw new NotImplementedException("Unrecognized parse style.")
            };

            this.parser.ThrowIfIllegal(arguments);
        }

        public IReadOnlySet<Argument> Arguments { get; }

        public ParseStyle Style { get; }

        public ParseResult Parse(IReadOnlyList<string> values) => this.parser.Parse(this.Arguments, values);
    }
}
