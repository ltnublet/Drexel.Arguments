using System;

namespace Drexel.Arguments.Parsers.Internals
{
    internal class ConditionResult : IEquatable<ConditionResult>, IEquatable<bool>
    {
        private readonly int value;

        private ConditionResult(int value)
        {
            this.value = value;
        }

        /// <summary>
        /// Indicates that the state should check the next transition.
        /// </summary>
        public static ConditionResult Continue { get; } = new ConditionResult(0);

        /// <summary>
        /// Indicates that the state machine should stop evaluating conditions, and transition into the state
        /// specified by this transition.
        /// </summary>
        public static ConditionResult Break { get; } = new ConditionResult(1);

        /// <summary>
        /// Indicates that the state machine should stop evaluating transitions.
        /// </summary>
        public static ConditionResult Stop { get; } = new ConditionResult(2);

        public static implicit operator bool(ConditionResult instance)
        {
            if (instance is null)
            {
                return false;
            }
            else
            {
                return instance.value == 0;
            }
        }

        public static implicit operator ConditionResult(bool instance)
        {
            if (instance)
            {
                return ConditionResult.Continue;
            }
            else
            {
                return ConditionResult.Break;
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
            return this.value switch
            {
                0 => other,
                1 => !other,
                2 => !other,
                _ => throw new InvalidOperationException("Unrecognized condition result internal value.")
            };
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }
    }
}
