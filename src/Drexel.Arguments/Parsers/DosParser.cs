using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Drexel.Collections.Generic;

namespace Drexel.Arguments.Parsers
{
    internal sealed class DosParser : IParser
    {
        private DosParser()
        {
        }

        public static DosParser Singleton { get; } = new DosParser();

        public void ThrowIfIllegal(IReadOnlySet<Argument> arguments)
        {
            HashSet<char> characters = new HashSet<char>();
            foreach (Argument argument in arguments)
            {
                foreach (string shortName in argument.ShortNames)
                {
                    if (shortName.Length > 1)
                    {
                        throw new ArgumentException(
                            "DOS-style parsing requires that short names be only one character long.");
                    }

                    char character = shortName[0];
                    if (character == '/')
                    {
                        throw new ArgumentException(
                            "DOS-style parsing requires names not start with '/'.");
                    }

                    if (!characters.Add(character))
                    {
                        throw new ArgumentException(
                            "DOS-style parsing requires that short names have a unique first character.");
                    }
                }
            }
        }

        public ParseResult Parse(
            IReadOnlySet<Argument> arguments,
            IReadOnlyList<string> values)
        {
            State nullOrEmpty = new State(
                "Null or empty",
                new Transition(
                    "Main",
                    (x, y) =>
                    {
                        throw new InvalidOperationException(
                            "Null or empty token encountered while parsing.");
                    }));
            State notEnoughOperands = new State(
                "Not enough operands",
                new Transition(
                    "Main",
                    (x, y) =>
                    {
                        throw new InvalidOperationException(
                            $"Argument '{y.CurrentArgument.HumanReadableName}' expected at least {y.CurrentArgument.OperandCount.LowerBound} values.");
                    }));
            State startsWithSlash = new State(
                "Starts with slash",
                new Transition(
                    "Value",
                    (x, y) =>
                    {
                        // If they just sent us "/", then that's actually a value.
                        string substring = x.Substring(1);
                        if (substring.Length == 0)
                        {
                            return true;
                        }

                        return false;
                    }),
                new Transition(
                    "Not enough operands",
                    (x, y) => y.CurrentArgument != null
                        && (y.Position - y.PositionAtTimeOfLastArgumentSet)
                            <= y.CurrentArgument.OperandCount.LowerBound),
                new Transition(
                    "Main",
                    (x, y) =>
                    {
                        string substring = x.Substring(1);
                        foreach (char character in substring)
                        {
                            bool recognized = false;
                            foreach (Argument argument in y.Arguments)
                            {
                                if (argument.ShortNames.Contains(character.ToString(CultureInfo.InvariantCulture)))
                                {
                                    if (y.CurrentArgument != argument)
                                    {
                                        y.Results.Order.Add(argument);
                                    }

                                    y.CurrentArgument = argument;

                                    recognized = true;
                                    break;
                                }
                            }

                            if (!recognized)
                            {
                                throw new InvalidOperationException(
                                    $"Unrecognized argument: {character}");
                            }
                        }

                        y.Position++;
                        return true;
                    }));
            State unparentedValue = new State(
                "Unparented value",
                new Transition(
                    "Main",
                    (x, y) =>
                    {
                        y.Results.UnparentedValues.Add(y.CurrentArgument, x);
                        y.Position++;
                        return true;
                    }));
            State value = new State(
                "Value",
                new Transition(
                    "Unparented value",
                    (x, y) =>
                    {
                        // If we don't have an argument, or our last argument ran out of values, then this is actually
                        // an unparented value.
                        if (y.CurrentArgument == null)
                        {
                            return true;
                        }
                        else if ((y.Position - y.PositionAtTimeOfLastArgumentSet)
                            > y.CurrentArgument.OperandCount.UpperBound)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }),
                new Transition(
                    "Main",
                    (x, y) =>
                    {
                        y.Results.ParentedValues.Add(y.CurrentArgument, x);
                        y.Position++;
                        return true;
                    }));

            State main = new State(
                "Main",
                new Transition(
                    "Null or empty",
                    (x, y) =>
                    {
                        return string.IsNullOrEmpty(x);
                    }),
                new Transition(
                    "Starts with slash",
                    (x, y) =>
                    {
                        return x[0] == '/';
                    }),
                new Transition(
                    "Value",
                    (x, y) =>
                    {
                        return y.CurrentArgument != null;
                    }),
                new Transition(
                    "Unparented value",
                    (x, y) =>
                    {
                        return true;
                    }));

            StateMachine stateMachine = new StateMachine(
                arguments,
                main,
                new List<State>()
                {
                    main,
                    nullOrEmpty,
                    startsWithSlash,
                    value,
                    unparentedValue,
                    notEnoughOperands
                });

            stateMachine.Run(values);

            return stateMachine.Results.ToParseResult();
        }

        private class MutableParseResult
        {
            public MutableParseResult()
            {
                this.Order = new List<Argument>();
                this.ParentedValues = new ParentedValues();
                this.UnparentedValues = new UnparentedValues();
            }

            public List<Argument> Order { get; }

            public ParentedValues ParentedValues { get; }

            public UnparentedValues UnparentedValues { get; }

            public ParseResult ToParseResult()
            {
                return new ParseResult(
                    this.Order,
                    this.ParentedValues.ToDictionary(),
                    this.UnparentedValues.ToList());
            }
        }

        private class StateMachine
        {
            private readonly IReadOnlyDictionary<string, State> states;
            private State currentState;
            private Argument? currentArgument;
            private int positionAtTimeOfLastArgumentSet;

            public StateMachine(
                IReadOnlySet<Argument> arguments,
                State initialState,
                IReadOnlyList<State> states)
            {
                this.Arguments = arguments;
                this.currentState = initialState;
                this.states = states.ToDictionary(x => x.Name, x => x);

                this.Position = 0;
                this.CurrentArgument = null;
                this.Results = new MutableParseResult();
                this.positionAtTimeOfLastArgumentSet = 0;
            }

            public IReadOnlySet<Argument> Arguments { get; }

            public MutableParseResult Results { get; }

            public int Position { get; set; }

            public int PositionAtTimeOfLastArgumentSet => this.positionAtTimeOfLastArgumentSet;

            public Argument? CurrentArgument
            {
                get => this.currentArgument;
                set
                {
                    this.currentArgument = value;
                    this.positionAtTimeOfLastArgumentSet = this.Position;
                }
            }

            public void Run(IReadOnlyList<string> values)
            {
                do
                {
                    foreach (Transition transition in this.currentState.Transitions)
                    {
                        if (transition.Condition.Invoke(values[this.Position], this))
                        {
                            this.currentState = this.states[transition.TransitionTo];
                            break;
                        }
                    }
                }
                while (this.Position < values.Count);

                if (this.currentArgument != null)
                {
                    if ((this.Position - this.PositionAtTimeOfLastArgumentSet) < this.CurrentArgument.OperandCount.LowerBound)
                    {
                        throw new InvalidOperationException(
                            $"Argument '{this.CurrentArgument.HumanReadableName}' expected at least {this.CurrentArgument.OperandCount.LowerBound} values.");
                    }
                }
            }
        }

        private class State
        {
            public State(
                string name,
                params Transition[] transitionParams)
                : this(name, transitions: transitionParams)
            {
            }

            public State(
                string name,
                IReadOnlyList<Transition> transitions)
            {
                this.Name = name;
                this.Transitions = transitions;
            }

            public string Name { get; }

            public IReadOnlyList<Transition> Transitions { get; }
        }

        private class Transition
        {
            public Transition(string transitionTo, Func<string, StateMachine, bool> condition)
            {
                this.Condition = condition;
                this.TransitionTo = transitionTo;
            }

            public Func<string, StateMachine, bool> Condition { get; }

            public string TransitionTo { get; }
        }
    }
}
