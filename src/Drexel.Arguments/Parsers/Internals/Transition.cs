using System;
using System.Collections.Generic;
using System.Text;

namespace Drexel.Arguments.Parsers.Internals
{
    internal class Transition<T>
    {
        public Transition(
            string name,
            params Condition<T>[] transitionParams)
            : this(name, transitions: transitionParams)
        {
        }

        public Transition(
            string name,
            IReadOnlyList<Condition<T>> transitions)
        {
            this.Name = name;
            this.Transitions = transitions;
        }

        public string Name { get; }

        public IReadOnlyList<Condition<T>> Transitions { get; }
    }
}
