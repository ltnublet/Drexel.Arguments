using System;

namespace Drexel.Arguments
{
    /// <summary>
    /// Represents the boundaries of a count of some scalar.
    /// </summary>
    public sealed class CountBounds : IEquatable<CountBounds>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=" CountBounds"/> class.
        /// </summary>
        /// <param name="lowerBound">
        /// The lower bound on the count allowed.
        /// </param>
        /// <param name="upperBound">
        /// When <see langword="null"/>, indicates that no upper bound exists. Otherwise, the maximumm allowed count.
        /// This value must be larger than <paramref name="lowerBound"/>, if it is non-<see langword="null"/>.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="lowerBound"/> is less than 0, or <paramref name="upperBound"/> is less than
        /// <paramref name="lowerBound"/>.
        /// </exception>
        public CountBounds(long lowerBound = 1, long? upperBound = null)
        {
            if (lowerBound < 0)
            {
                throw new ArgumentException(
                    "Lower bound cannot be less than zero.",
                    nameof(lowerBound));
            }

            if (upperBound.HasValue && upperBound.Value < lowerBound)
            {
                throw new ArgumentException(
                    "Upper bound of allowed count cannot be less than lower bound.",
                    nameof(upperBound));
            }

            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
        }

        /// <summary>
        /// Gets a <see cref="CountBounds"/> that corresponds to a count of exactly zero (AKA, a "flag").
        /// </summary>
        public static CountBounds Flag { get; } = new CountBounds(0, 0);

        /// <summary>
        /// Gets a <see cref="CountBounds"/> that corresponds to a count of exactly one.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Naming",
            "CA1720:Identifier contains type name",
            Justification = "Doesn't apply to this situation")]
        public static CountBounds Single { get; } = new CountBounds(1, 1);

        /// <summary>
        /// Gets a <see cref="CountBounds"/> that corresponds to a count of one or more.
        /// </summary>
        public static CountBounds OneOrMore { get; } = new CountBounds(1, null);

        /// <summary>
        /// Gets a <see cref="CountBounds"/> that corresponds to a count of zero or more.
        /// </summary>
        public static CountBounds ZeroOrMore { get; } = new CountBounds(0, null);

        /// <summary>
        /// Gets the lower bound of the count.
        /// </summary>
        public long LowerBound { get; }

        /// <summary>
        /// Gets the upper bound of the count, if one exists.
        /// </summary>
        public long? UpperBound { get; }

        /// <summary>
        /// Compares two <see cref="CountBounds"/>s.
        /// </summary>
        /// <param name="left">
        /// An <see cref="CountBounds"/> to compare.
        /// </param>
        /// <param name="right">
        /// An <see cref="CountBounds"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// The equivalent method for this operator is <see cref="CountBounds.Equals(CountBounds)"/>.
        /// </remarks>
        public static bool operator ==(CountBounds left, CountBounds right)
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
        /// Compares two <see cref="CountBounds"/>s.
        /// </summary>
        /// <param name="left">
        /// An <see cref="CountBounds"/> to compare.
        /// </param>
        /// <param name="right">
        /// An <see cref="CountBounds"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> differ; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// The equivalent method for this operator is <see cref="CountBounds.Equals(CountBounds)"/>.
        /// </remarks>
        public static bool operator !=(CountBounds left, CountBounds right)
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
            if (obj is CountBounds asOperandCount)
            {
                return this.Equals(asOperandCount);
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="CountBounds"/>
        /// <paramref name="other"/>.
        /// </summary>
        /// <param name="other">
        /// An <see cref="CountBounds"/> to compare with this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if this instance was equal to the specified <see cref="CountBounds"/>
        /// <paramref name="other"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(CountBounds other)
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
