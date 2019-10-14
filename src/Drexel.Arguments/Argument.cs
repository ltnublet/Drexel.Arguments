using System;
using Drexel.Collections.Generic;

namespace Drexel.Arguments
{
    /// <summary>
    /// Represents an argument (something also called an option).
    /// </summary>
    public abstract class Argument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Argument"/> class.
        /// </summary>
        /// <param name="shortNames">
        /// The short names of the argument.
        /// </param>
        /// <param name="longNames">
        /// The long names of the argument.
        /// </param>
        /// <param name="description">
        /// The description of the argument.
        /// </param>
        /// <param name="required">
        /// Indicates whether the argument is required.
        /// </param>
        /// <param name="operandCount">
        /// Operand count limitations associated with this argument.
        /// </param>
        /// <param name="dependsOn">
        /// Arguments that this argument depends on.
        /// </param>
        /// <param name="exclusiveWith">
        /// Arguments that this argument is exclusive with.
        /// </param>
        public Argument(
            IReadOnlySet<string> shortNames,
            IReadOnlySet<string> longNames,
            string description,
            bool required,
            OperandCount operandCount,
            IReadOnlyInvariantSet<Argument> dependsOn,
            IReadOnlyInvariantSet<Argument> exclusiveWith)
        {
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

            if (null == dependsOn)
            {
                throw new ArgumentNullException(nameof(dependsOn));
            }

            if (null == exclusiveWith)
            {
                throw new ArgumentNullException(nameof(exclusiveWith));
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

            if (dependsOn.Overlaps(exclusiveWith))
            {
                throw new ArgumentException("Argument cannot depend on an argument it is exclusive with.");
            }

            this.ShortNames = shortNames;
            this.LongNames = longNames;
            this.Description = description;
            this.Required = required;
            this.OperandCount = operandCount;
            this.DependsOn = dependsOn;
            this.ExclusiveWith = exclusiveWith;
        }

        /// <summary>
        /// Gets the short name(s) of this argument.
        /// </summary>
        public IReadOnlySet<string> ShortNames { get; }

        /// <summary>
        /// Gets the long name(s) of this argument.
        /// </summary>
        public IReadOnlySet<string> LongNames { get; }

        /// <summary>
        /// Gets the description of this argument.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets a value indicating whether this argument is required.
        /// </summary>
        public bool Required { get; }

        /// <summary>
        /// Gets the <see cref="Arguments.OperandCount"/> of this argument, indicating restrictions on the number of operands
        /// this argument can be satisfied by.
        /// </summary>
        public OperandCount OperandCount { get; }

        /// <summary>
        /// Gets the <see cref="Argument"/>s this argument depends on.
        /// </summary>
        public IReadOnlyInvariantSet<Argument> DependsOn { get; }

        /// <summary>
        /// Gets the <see cref="Argument"/>s this argument is exclusive with.
        /// </summary>
        public IReadOnlyInvariantSet<Argument> ExclusiveWith { get; }
    }
}
