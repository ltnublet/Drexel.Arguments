using System;
using System.Diagnostics;
using Drexel.Collections.Generic;

namespace Drexel.Arguments
{
    /// <summary>
    /// Represents an argument (something also called an option).
    /// </summary>
    [DebuggerDisplay("{HumanReadableName,nq}")]
    public abstract class Argument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Argument"/> class.
        /// </summary>
        /// <param name="humanReadableName">
        /// The human-readable name of this argument. This name will appear in error messages or help pages, but will
        /// not be used during parsing.
        /// </param>
        /// <param name="shortNames">
        /// The short names of the argument.
        /// </param>
        /// <param name="longNames">
        /// The long names of the argument.
        /// </param>
        /// <param name="description">
        /// The description of the argument.
        /// </param>
        /// <param name="operandCount">
        /// Operand count limitations associated with this argument.
        /// </param>
        public Argument(
            string humanReadableName,
            IReadOnlyInvariantSet<string> shortNames,
            IReadOnlyInvariantSet<string> longNames,
            string description,
            CountBounds operandCount)
        {
            if (null == humanReadableName)
            {
                throw new ArgumentNullException(nameof(humanReadableName));
            }

            if (null == shortNames)
            {
                throw new ArgumentNullException(nameof(shortNames));
            }

            if (null == longNames)
            {
                throw new ArgumentNullException(nameof(longNames));
            }

            if (null == description)
            {
                throw new ArgumentNullException(nameof(description));
            }

            if (null == operandCount)
            {
                throw new ArgumentNullException(nameof(operandCount));
            }

            foreach (string name in shortNames)
            {
                if (name.Length < 1)
                {
                    throw new ArgumentException("Short name must be at least one character long.");
                }
            }

            foreach (string name in longNames)
            {
                if (name.Length < 1)
                {
                    throw new ArgumentException("Long name must be at least one character long.");
                }
            }

            this.HumanReadableName = humanReadableName;
            this.ShortNames = shortNames;
            this.LongNames = longNames;
            this.Description = description;
            this.OperandCount = operandCount;
        }

        /// <summary>
        /// Gets the human readable name of this argument. This name will be used in error messages or help pages, but
        /// will not be used during parsing.
        /// </summary>
        public string HumanReadableName { get; }

        /// <summary>
        /// Gets the short name(s) of this argument.
        /// </summary>
        public IReadOnlyInvariantSet<string> ShortNames { get; }

        /// <summary>
        /// Gets the long name(s) of this argument.
        /// </summary>
        public IReadOnlyInvariantSet<string> LongNames { get; }

        /// <summary>
        /// Gets the description of this argument.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the <see cref="Arguments.CountBounds"/> of this argument, indicating the minimum and maximum count of
        /// operands required to satisfy this argument.
        /// </summary>
        public CountBounds OperandCount { get; }
    }
}
