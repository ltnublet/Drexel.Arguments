using System;
using System.Collections.Generic;
using System.Text;

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
            ////foreach (KeyValuePair<Argument, ArgumentCallbacks> kvp in this.callbacks)
            ////{
            ////    if (results.ParentedValues.TryGetValue(kvp.Key, out IReadOnlyList<string> values))
            ////    {

            ////    }
            ////}
        }
    }
}
