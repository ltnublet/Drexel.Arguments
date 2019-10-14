using System;
using System.Collections.Generic;
using System.Linq;

namespace Drexel.Arguments.Parsers.Internals
{
    internal sealed class StateMachine<T>
    {
        private readonly IReadOnlyDictionary<string, Transition<T>> states;
        private readonly Transition<T> startInState;

        public StateMachine(
            Transition<T> startInState,
            IReadOnlyList<Transition<T>> states)
        {
            this.startInState = startInState;
            this.states = states.ToDictionary(x => x.Name, x => x);
        }

        public T Run(Func<T> sharedStateFactory)
        {
            Transition<T> currentState = this.startInState;
            T sharedState = sharedStateFactory.Invoke();

            bool running = true;
            top: while (running)
            {
                foreach (Condition<T> condition in currentState.Transitions)
                {
                    ConditionResult result = condition.Conditional.Invoke(sharedState);
                    if (result == ConditionResult.Break)
                    {
                        currentState = this.states[condition.TransitionTo];
                        goto top;
                    }
                    else if (result == ConditionResult.Continue)
                    {
                        continue;
                    }
                    else if (result == ConditionResult.Stop)
                    {
                        running = false;
                        goto top;
                    }
                    else
                    {
                        throw new InvalidOperationException("Unrecognized condition result.");
                    }
                }
            }

            return sharedState;
        }
    }
}
