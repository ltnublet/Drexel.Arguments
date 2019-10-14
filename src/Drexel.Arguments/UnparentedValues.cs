using System.Collections.Generic;
using System.Linq;

namespace Drexel.Arguments
{
    internal sealed class UnparentedValues
    {
        private readonly List<string> argumentless;
        private readonly Dictionary<int, List<string>> values;
        private readonly List<Argument> arguments;

        public UnparentedValues()
        {
            this.argumentless = new List<string>();
            this.values = new Dictionary<int, List<string>>();
            this.arguments = new List<Argument>();
        }

        public void Add(Argument? argument, string value)
        {
            if (argument == null)
            {
                this.argumentless.Add(value);
            }
            else
            {
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
        }

        public IReadOnlyList<KeyValuePair<Argument?, IReadOnlyList<string>>> ToList()
        {
            List<KeyValuePair<Argument?, IReadOnlyList<string>>> result =
                new List<KeyValuePair<Argument?, IReadOnlyList<string>>>(this.arguments.Count);
            if (this.argumentless.Count > 0)
            {
                result.Add(new KeyValuePair<Argument?, IReadOnlyList<string>>(null, this.argumentless));
            }

            for (int counter = 0; counter < this.arguments.Count; counter++)
            {
                result.Add(
                    new KeyValuePair<Argument?, IReadOnlyList<string>>(
                        this.arguments[counter],
                        this.values[counter]));
            }

            return result;
        }
    }
}
