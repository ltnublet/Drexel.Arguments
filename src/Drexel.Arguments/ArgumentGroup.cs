using System;
using System.Collections.Generic;

namespace Drexel.Arguments
{
    public class ArgumentGroup
    {
        private readonly IReadOnlyDictionary<Argument, ArgumentCallbacks> callbacks;

        public ArgumentGroup(IReadOnlyDictionary<Argument, ArgumentCallbacks> callbacks)
        {
            this.callbacks = callbacks ?? throw new ArgumentNullException(nameof(callbacks));
        }

        public void Invoke(ParseResult results)
        {
            foreach (KeyValuePair<Argument, ArgumentCallbacks> kvp in this.callbacks)
            {
                if (results.ParentedValues.ContainsKey(kvp.Key))
                {
                    kvp.Value.OnSupplied?.Invoke(kvp.Key, results);
                }
                else
                {
                    kvp.Value.OnOmitted?.Invoke(kvp.Key, results);
                }
            }
        }
    }
}
