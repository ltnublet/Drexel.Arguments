using System;
using System.Collections.Generic;
using System.Linq;

namespace Drexel.Arguments.Parsers.Internals
{
    internal sealed class StateMachine<T>
    {
        private readonly IReadOnlyDictionary<string, Transition<T>> states;
        private readonly Transition<T> startInState;
        private readonly Func<T> sharedStateFactory;

        public StateMachine(
            Transition<T> startInState,
            IReadOnlyList<Transition<T>> states,
            Func<T> sharedStateFactory)
        {
            this.startInState = startInState;
            this.states = states.ToDictionary(x => x.Name, x => x);
            this.sharedStateFactory = sharedStateFactory;
        }

        public T Run()
        {
            Transition<T> currentState = this.startInState;
            T sharedState = this.sharedStateFactory.Invoke();

            bool running = true;
            do
            {
                foreach (Condition<T> condition in currentState.Transitions)
                {
                    ConditionResult result = condition.Conditional.Invoke(sharedState);
                    if (result == ConditionResult.Break)
                    {
                        currentState = this.states[condition.TransitionTo];
                        goto bottom;
                    }
                    else if (result == ConditionResult.Continue)
                    {
                        goto bottom;
                    }
                    else if (result == ConditionResult.Stop)
                    {
                        running = false;
                        goto bottom;
                    }
                    else
                    {
                        throw new InvalidOperationException("Unrecognized condition result.");
                    }
                }

                bottom:
                {
                }
            }
            while (running);

            return sharedState;
        }
    }
}
