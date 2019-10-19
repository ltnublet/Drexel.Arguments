using System;
using System.Collections.Generic;
using System.Linq;

namespace Drexel.Arguments.Parsers.Internals
{
    internal sealed class UnparentedValues
    {
        private readonly List<string> front;
        private readonly List<string> back;
        private readonly Dictionary<int, List<string>> values;
        private readonly List<Argument> arguments;

        public UnparentedValues()
        {
            this.front = new List<string>();
            this.back = new List<string>();
            this.values = new Dictionary<int, List<string>>();
            this.arguments = new List<Argument>();
        }

        public void Add(Position position, string value)
        {
            switch (position)
            {
                case Position.Front:
                    this.front.Add(value);
                    break;
                case Position.Back:
                    this.back.Add(value);
                    break;
                default:
                    throw new ArgumentException("Unrecognized position.", nameof(position));
            }
        }

        public void Add(Argument argument, string value)
        {
            if (argument is null)
            {
                throw new ArgumentNullException(nameof(argument));
            }

            if (this.arguments.LastOrDefault() != argument)
            {
                this.arguments.Add(argument);
            }

            int index = this.arguments.Count - 1;
            if (!this.values.TryGetValue(index, out List<string> list))
            {
                list = new List<string>();
                this.values.Add(index, list);
            }

            list.Add(value);
        }

        public IReadOnlyList<KeyValuePair<Argument?, IReadOnlyList<string>>> ToList()
        {
            List<KeyValuePair<Argument?, IReadOnlyList<string>>> result =
                new List<KeyValuePair<Argument?, IReadOnlyList<string>>>(this.arguments.Count);
            if (this.front.Count > 0)
            {
                result.Add(new KeyValuePair<Argument?, IReadOnlyList<string>>(null, this.front));
            }

            for (int counter = 0; counter < this.arguments.Count; counter++)
            {
                result.Add(
                    new KeyValuePair<Argument?, IReadOnlyList<string>>(
                        this.arguments[counter],
                        this.values[counter]));
            }

            if (this.back.Count > 0)
            {
                result.Add(new KeyValuePair<Argument?, IReadOnlyList<string>>(null, this.back));
            }

            return result;
        }
    }
}
