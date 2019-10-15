using System;
using System.Collections.Generic;
using System.Linq;

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
        /// <param name="parentedValues">
        /// A mapping between arguments and their parsed results.
        /// </param>
        /// <param name="unparentedValues">
        /// Any unparented values that were found during parsing.
        /// </param>
        public ParseResult(
            IReadOnlyList<Argument> order,
            IReadOnlyDictionary<Argument, IReadOnlyList<IReadOnlyList<string>>> parentedValues,
            IReadOnlyList<KeyValuePair<Argument?, IReadOnlyList<string>>> unparentedValues)
        {
            this.Order = order ?? throw new ArgumentNullException(nameof(order));
            this.ParentedValues = parentedValues ?? throw new ArgumentNullException(nameof(parentedValues));
            this.UnparentedValues = unparentedValues ?? throw new ArgumentNullException(nameof(unparentedValues));
        }

        /// <summary>
        /// Gets the order in which the arguments were received.
        /// </summary>
        public IReadOnlyList<Argument> Order { get; }

        /// <summary>
        /// Gets the mapping between arguments and their parsed results.
        /// </summary>
        public IReadOnlyDictionary<Argument, IReadOnlyList<IReadOnlyList<string>>> ParentedValues { get; }

        /// <summary>
        /// Gets any unparented values that were found during parsing. Note that a key of null means that the
        /// unparented value(s) occurred before any argument was specified.
        /// </summary>
        public IReadOnlyList<KeyValuePair<Argument?, IReadOnlyList<string>>> UnparentedValues { get; }
    }
}
