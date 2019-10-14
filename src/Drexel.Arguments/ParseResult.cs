using System;
using System.Collections.Generic;

namespace Drexel.Arguments
{
    /// <summary>
    /// Represents a collection of parse results.
    /// </summary>
    public sealed class ParseResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParseResult"/> class.
        /// </summary>
        /// <param name="order">
        /// The order in which the arguments were received.
        /// </param>
        /// <param name="arguments">
        /// A mapping between arguments and their parsed results.
        /// </param>
        /// <param name="unparentedValues">
        /// Any unparented values that were found during parsing.
        /// </param>
        public ParseResult(
            IReadOnlyList<Argument> order,
            IReadOnlyDictionary<Argument, IReadOnlyList<string>> arguments,
            IReadOnlyList<KeyValuePair<Argument, IReadOnlyList<string>>> unparentedValues)
        {
            this.Order = order ?? throw new ArgumentNullException(nameof(order));
            this.Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
            this.UnparentedValues = unparentedValues ?? throw new ArgumentNullException(nameof(unparentedValues));
        }

        /// <summary>
        /// Gets the mapping between arguments and their parsed results.
        /// </summary>
        public IReadOnlyDictionary<Argument, IReadOnlyList<string>> Arguments { get; }

        /// <summary>
        /// Gets the order in which the arguments were received.
        /// </summary>
        public IReadOnlyList<Argument> Order { get; }

        /// <summary>
        /// Gets any unparented values that were found during parsing.
        /// </summary>
        public IReadOnlyList<KeyValuePair<Argument, IReadOnlyList<string>>> UnparentedValues { get; }
    }
}
