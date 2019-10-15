using System;

namespace Drexel.Arguments
{
    public class ArgumentCallbacks
    {
        public ArgumentCallbacks(
            Action<ParseResult>? onSupplied = null,
            Action<ParseResult>? onOmitted = null)
        {
            this.OnSupplied = onSupplied;
            this.OnOmitted = onOmitted;
        }

        public Action<ParseResult>? OnSupplied { get; }

        public Action<ParseResult>? OnOmitted { get; }
    }
}
