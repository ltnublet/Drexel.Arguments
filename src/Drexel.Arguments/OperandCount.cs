using System;

namespace Drexel.Arguments
{
    /// <summary>
    /// Represents the operand count for an argument.
    /// </summary>
    public sealed class OperandCount : IEquatable<OperandCount>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=" OperandCount"/> class.
        /// </summary>
        /// <param name="lowerBound">
        /// The lower bound on the number of operands allowed.
        /// </param>
        /// <param name="upperBound">
        /// When <see langword="null"/>, indicates that no upper bound on the number of operands exists. Otherwise, the
        /// maximumm allowed number of operands. This value must be larger than <paramref name="lowerBound"/>, if it is
        /// non-<see langword="null"/>.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="upperBound"/> is less than <paramref name="lowerBound"/>.
        /// </exception>
        public OperandCount(ulong lowerBound = 1, ulong? upperBound = null)
        {
            if (upperBound.HasValue && upperBound.Value < lowerBound)
            {
                throw new ArgumentException(
                    "Upper bound of allowed operand count cannot be less than lower bound.",
                    nameof(upperBound));
            }

            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
        }

        /// <summary>
        /// Gets an <see cref="OperandCount"/> that corresponds to 0 operands (AKA, a "flag").
        /// </summary>
        public static OperandCount Flag { get; } = new OperandCount(0, 0);

        /// <summary>
        /// Gets an <see cref="OperandCount"/> that corresponds to a single operand.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Naming",
            "CA1720:Identifier contains type name",
            Justification = "Doesn't apply to this situation")]
        public static OperandCount Single { get; } = new OperandCount(1, 1);

        /// <summary>
        /// Gets the lower bound on the allowed number of operands.
        /// </summary>
        public ulong LowerBound { get; }

        /// <summary>
        /// Gets the upper bound on the allowed number of operands, if one exists.
        /// </summary>
        public ulong? UpperBound { get; }

        /// <summary>
        /// Compares two <see cref="OperandCount"/>s.
        /// </summary>
        /// <param name="left">
        /// An <see cref="OperandCount"/> to compare.
        /// </param>
        /// <param name="right">
        /// An <see cref="OperandCount"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// The equivalent method for this operator is <see cref="OperandCount.Equals(OperandCount)"/>.
        /// </remarks>
        public static bool operator ==(OperandCount left, OperandCount right)
        {
            if (left is null)
            {
                return right is null;
            }
            else
            {
                return left.Equals(right);
            }
        }

        /// <summary>
        /// Compares two <see cref="OperandCount"/>s.
        /// </summary>
        /// <param name="left">
        /// An <see cref="OperandCount"/> to compare.
        /// </param>
        /// <param name="right">
        /// An <see cref="OperandCount"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> differ; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// The equivalent method for this operator is <see cref="OperandCount.Equals(OperandCount)"/>.
        /// </remarks>
        public static bool operator !=(OperandCount left, OperandCount right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="object"/>
        /// <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">
        /// An <see cref="object"/> to compare with this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if this instance was equal to the specified <see cref="object"/>
        /// <paramref name="obj"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is OperandCount asOperandCount)
            {
                return this.Equals(asOperandCount);
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="OperandCount"/>
        /// <paramref name="other"/>.
        /// </summary>
        /// <param name="other">
        /// An <see cref="OperandCount"/> to compare with this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if this instance was equal to the specified <see cref="OperandCount"/>
        /// <paramref name="other"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(OperandCount other)
        {
            if (other is null)
            {
                return false;
            }

            if (this.UpperBound.HasValue)
            {
                return this.LowerBound == other.LowerBound
                    && other.UpperBound.HasValue
                    && this.UpperBound.Value == other.UpperBound.Value;
            }
            else
            {
                return this.LowerBound == other.LowerBound
                    && !other.UpperBound.HasValue;
            }
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// The hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 75557;
                hash = (31 * hash) + this.UpperBound.GetHashCode();
                hash = (31 * hash) + this.LowerBound.GetHashCode();

                return hash;
            }
        }
    }
}
