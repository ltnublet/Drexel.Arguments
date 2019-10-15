using System;
using System.Diagnostics;

namespace Drexel.Arguments.Parsers.Internals
{
    [DebuggerDisplay("{debugName,nq}")]
    internal class ConditionResult : IEquatable<ConditionResult>, IEquatable<bool>
    {
        private readonly int value;
        private readonly string debugName;

        private ConditionResult(int value, string debugName)
        {
            this.value = value;
            this.debugName = debugName;
        }

        /// <summary>
        /// Gets a <see cref="ConditionResult"/> indicating that the state should check the next transition.
        /// </summary>
        public static ConditionResult Continue { get; } = new ConditionResult(0, nameof(Continue));

        /// <summary>
        /// Gets a <see cref="ConditionResult"/> indicating that the state machine should stop evaluating conditions,
        /// and should transition into the state specified by this transition.
        /// </summary>
        public static ConditionResult Break { get; } = new ConditionResult(1, nameof(Break));

        /// <summary>
        /// Gets a <see cref="ConditionResult"/> indicating that that the state machine should stop evaluating
        /// transitions.
        /// </summary>
        public static ConditionResult Stop { get; } = new ConditionResult(2, nameof(Stop));

        public static implicit operator bool(ConditionResult instance)
        {
            if (instance is null)
            {
                return false;
            }
            else
            {
                return instance != ConditionResult.Continue;
            }
        }

        public static implicit operator ConditionResult(bool instance)
        {
            if (instance)
            {
                return ConditionResult.Break;
            }
            else
            {
                return ConditionResult.Continue;
            }
        }

        public static bool operator ==(ConditionResult left, ConditionResult right)
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

        public static bool operator !=(ConditionResult left, ConditionResult right)
        {
            return !(left == right);
        }

        public static bool operator ==(ConditionResult left, bool right)
        {
            if (left is null)
            {
                return !right;
            }
            else
            {
                return left.Equals(right);
            }
        }

        public static bool operator !=(ConditionResult left, bool right)
        {
            return !(left == right);
        }

        public static bool operator ==(bool left, ConditionResult right)
        {
            if (right is null)
            {
                return !left;
            }
            else
            {
                return right.Equals(left);
            }
        }

        public static bool operator !=(bool left, ConditionResult right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is bool asBool)
            {
                return this.Equals(asBool);
            }
            else if (obj is ConditionResult asResult)
            {
                return this.Equals(asResult);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(ConditionResult other)
        {
            if (other is null)
            {
                return false;
            }
            else
            {
                return this.value == other.value;
            }
        }

        public bool Equals(bool other)
        {
            return (bool)this == other;
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }
    }
}
