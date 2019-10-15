using System;
using System.Diagnostics;

namespace Drexel.Arguments.Parsers.Internals
{
    [DebuggerDisplay("{TransitionTo,nq}")]
    internal class Condition<T>
    {
        public Condition(string transitionTo, Func<T, ConditionResult> condition)
        {
            this.Conditional = condition;
            this.TransitionTo = transitionTo;
        }

        public Func<T, ConditionResult> Conditional { get; }

        public string TransitionTo { get; }
    }
}
