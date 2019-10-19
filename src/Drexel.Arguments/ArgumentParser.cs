using System;
using System.Collections.Generic;
using Drexel.Arguments.Parsers;
using Drexel.Arguments.Parsers.Implementations;
using Drexel.Collections.Generic;

namespace Drexel.Arguments
{
    public sealed class ArgumentParser
    {
        private static readonly IReadOnlyInvariantSet<ParseStyle> NotImplementedStyles =
            new SetAdapter<ParseStyle>(new HashSet<ParseStyle>())
            {
                ParseStyle.GNU,
                ParseStyle.Unity,
                ParseStyle.Windows
            };

        private readonly IParser parser;

        public ArgumentParser(ParseStyle style, IReadOnlyInvariantSet<Argument> arguments)
        {
            if (ArgumentParser.NotImplementedStyles.Contains(style))
            {
                throw new NotImplementedException("Specified parsing style is not yet implemented.");
            }

            this.Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
            this.Style = style;

            this.parser = style switch
            {
                ParseStyle.DOS => DosParser.Singleton,
                ParseStyle.GNU => GnuParser.Singleton,
                ParseStyle.MSBuild => MsBuildParser.Singleton,
                ParseStyle.Posix => PosixParser.Singleton,
                ParseStyle.Unity => UnityParser.Singleton,
                ParseStyle.Windows => WindowsParser.Singleton,
                _ => throw new NotImplementedException("Unrecognized parse style.")
            };

            this.parser.ThrowIfIllegal(arguments);
        }

        public IReadOnlyInvariantSet<Argument> Arguments { get; }

        public ParseStyle Style { get; }

        public ParseResult Parse(IReadOnlyList<string> values) => this.parser.Parse(this.Arguments, values);
    }
}
