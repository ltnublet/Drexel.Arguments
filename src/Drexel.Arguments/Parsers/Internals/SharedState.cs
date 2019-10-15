using System.Collections.Generic;
using Drexel.Collections.Generic;

namespace Drexel.Arguments.Parsers.Internals
{
    internal class SharedState
    {
        private Argument? currentArgument;
        private string? currentValueOverride;
        private int position;

        public SharedState(
            IReadOnlySet<Argument> arguments,
            IReadOnlyList<string> values)
        {
            this.Arguments = arguments;
            this.Values = values;

            this.currentArgument = null;
            this.currentValueOverride = null;
            this.position = 0;

            this.Results = new MutableParseResult();
            this.Position = 0;
            this.PositionAtTimeOfLastArgumentSet = 0;
        }

        public IReadOnlySet<Argument> Arguments { get; }

        public IReadOnlyList<string> Values { get; }

        public MutableParseResult Results { get; }

        public int Position
        {
            get => this.position;
            set
            {
                this.position = value;
                this.currentValueOverride = null;
            }
        }

        public int PositionAtTimeOfLastArgumentSet { get; private set; }

        public Argument? CurrentArgument
        {
            get => this.currentArgument;
            set
            {
                if (this.currentArgument != null && this.currentArgument != value)
                {
                    this.Results.ParentedValues.ExplicitAdvance();
                }

                this.currentArgument = value;
                this.PositionAtTimeOfLastArgumentSet = this.Position;
            }
        }

        public string CurrentValue
        {
            get
            {
                if (this.currentValueOverride is null)
                {
                    return this.Values[this.Position];
                }
                else
                {
                    return this.currentValueOverride;
                }
            }
            set => this.currentValueOverride = value;
        }
    }
}
