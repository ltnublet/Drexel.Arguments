using System.Collections.Generic;
using Drexel.Collections.Generic;

namespace Drexel.Arguments.Parsers.Internals
{
    internal class SharedState
    {
        private Argument? currentArgument;

        public SharedState(
            IReadOnlySet<Argument> arguments,
            IReadOnlyList<string> values)
        {
            this.Arguments = arguments;
            this.Values = values;

            this.currentArgument = null;
            this.Results = new MutableParseResult();
            this.Position = 0;
            this.PositionAtTimeOfLastArgumentSet = 0;
        }

        public IReadOnlySet<Argument> Arguments { get; }

        public IReadOnlyList<string> Values { get; }

        public MutableParseResult Results { get; }

        public int Position { get; set; }

        public int PositionAtTimeOfLastArgumentSet { get; private set; }

        public Argument? CurrentArgument
        {
            get => this.currentArgument;
            set
            {
                this.currentArgument = value;
                this.PositionAtTimeOfLastArgumentSet = this.Position;
            }
        }

        public string CurrentValue => this.Values[this.Position];
    }
}
