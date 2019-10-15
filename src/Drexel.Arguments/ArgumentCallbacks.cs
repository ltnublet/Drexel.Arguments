using System;

namespace Drexel.Arguments
{
    public class ArgumentCallbacks
    {
        public ArgumentCallbacks(
            Action<Argument, ParseResult>? onSupplied = null,
            Action<Argument, ParseResult>? onOmitted = null)
        {
            this.OnSupplied = onSupplied;
            this.OnOmitted = onOmitted;
        }

        public Action<Argument, ParseResult>? OnSupplied { get; }

        public Action<Argument, ParseResult>? OnOmitted { get; }
    }
}
