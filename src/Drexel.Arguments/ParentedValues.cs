using System.Collections.Generic;
using System.Linq;

namespace Drexel.Arguments
{
    internal sealed class ParentedValues
    {
        private readonly Dictionary<Argument, List<string>> values;

        public ParentedValues()
        {
            this.values = new Dictionary<Argument, List<string>>();
        }

        public void Add(Argument argument, string value)
        {
            if (!this.values.TryGetValue(argument, out List<string> list))
            {
                list = new List<string>();
                this.values.Add(argument, list);
            }

            list.Add(value);
        }

        public IReadOnlyDictionary<Argument, IReadOnlyList<string>> ToDictionary()
        {
            return this
                .values
                .ToDictionary<KeyValuePair<Argument, List<string>>, Argument, IReadOnlyList<string>>(
                    x => x.Key,
                    x => x.Value);
        }
    }
}
