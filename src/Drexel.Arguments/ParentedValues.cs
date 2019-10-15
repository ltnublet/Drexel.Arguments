using System.Collections.Generic;
using System.Linq;

namespace Drexel.Arguments
{
    internal sealed class ParentedValues
    {
        private readonly Dictionary<Argument, List<List<string>>> values;
        private Argument? lastArgument;

        public ParentedValues()
        {
            this.values = new Dictionary<Argument, List<List<string>>>();
            this.lastArgument = null;
        }

        public void Add(Argument argument, string value)
        {
            List<string> last = default;
            if (!this.values.TryGetValue(argument, out List<List<string>> lists))
            {
                lists = new List<List<string>>();
                last = new List<string>();
                lists.Add(last);
                this.values.Add(argument, lists);
            }

            if (this.lastArgument != argument)
            {
                this.lastArgument = argument;
                if (last == default)
                {
                    last = new List<string>();
                    lists.Add(last);
                }
            }
            else
            {
                last = lists.Last();
            }

            last.Add(value);
        }

        public void ExplicitAdvance()
        {
            this.lastArgument = null;
        }

        public IReadOnlyDictionary<Argument, IReadOnlyList<IReadOnlyList<string>>> ToDictionary()
        {
            return this
                .values
                .ToDictionary<KeyValuePair<Argument, List<List<string>>>, Argument, IReadOnlyList<IReadOnlyList<string>>>(
                    x => x.Key,
                    x => x.Value);
        }
    }
}
