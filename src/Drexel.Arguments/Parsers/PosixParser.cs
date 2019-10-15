using System;
using System.Collections.Generic;
using System.Globalization;
using Drexel.Arguments.Parsers.Internals;
using Drexel.Collections.Generic;

namespace Drexel.Arguments.Parsers
{
    internal sealed class PosixParser : IParser
    {
        private static readonly StateMachine<SharedState> StateMachine;

        static PosixParser()
        {
            Transition<SharedState> notEnoughOperands = new Transition<SharedState>(
                "Not enough operands",
                new Condition<SharedState>(
                    "Main",
                    x =>
                    {
                        throw new InvalidOperationException(
                            $"Argument '{x.CurrentArgument.HumanReadableName}' expected at least {x.CurrentArgument.OperandCount.LowerBound} values.");
                    }));
            Transition<SharedState> nullOrEmpty = new Transition<SharedState>(
                "Null or empty",
                new Condition<SharedState>(
                    "Main",
                    x =>
                    {
                        throw new InvalidOperationException(
                            "Null or empty token encountered while parsing.");
                    }));
            Transition<SharedState> noMoreValues = new Transition<SharedState>(
                "No more values",
                new Condition<SharedState>(
                    "Not enough operands",
                    x => x.CurrentArgument != null
                        && (x.Position - x.PositionAtTimeOfLastArgumentSet)
                            <= x.CurrentArgument.OperandCount.LowerBound),
                new Condition<SharedState>(
                    "Main",
                    x =>
                    {
                        return ConditionResult.Stop;
                    }));
            Transition<SharedState> readToEnd = new Transition<SharedState>(
                "Read to end",
                new Condition<SharedState>(
                    "No more values",
                    x =>
                    {
                        return x.Position >= x.Values.Count;
                    }),
                new Condition<SharedState>(
                    "Read to end",
                    x =>
                    {
                        x.Results.UnparentedValues.Add(
                            Position.Back,
                            x.CurrentValue);

                        x.Position++;
                        return true;
                    }));
            Transition<SharedState> startsWithHyphen = new Transition<SharedState>(
                "Starts with hyphen",
                new Condition<SharedState>(
                    "Read to end",
                    x =>
                    {
                        if (x.CurrentValue == "--")
                        {
                            x.Position++;
                            return true;
                        }

                        return false;
                    }),
                new Condition<SharedState>(
                    "Value",
                    x =>
                    {
                        // If they just sent us "-", then that's actually a value.
                        string substring = x.CurrentValue.Substring(1);
                        if (substring.Length == 0)
                        {
                            return true;
                        }

                        return false;
                    }),
                new Condition<SharedState>(
                    "Not enough operands",
                    x => x.CurrentArgument != null
                        && (x.Position - x.PositionAtTimeOfLastArgumentSet)
                            <= x.CurrentArgument.OperandCount.LowerBound),
                new Condition<SharedState>(
                    "Main",
                    x =>
                    {
                        string substring = x.CurrentValue.Substring(1);
                        foreach (char character in substring)
                        {
                            bool recognized = false;
                            foreach (Argument argument in x.Arguments)
                            {
                                if (argument.ShortNames.Contains(character.ToString(CultureInfo.InvariantCulture)))
                                {
                                    if (x.CurrentArgument != argument)
                                    {
                                        x.Results.Order.Add(argument);
                                    }

                                    x.CurrentArgument = argument;

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

                        x.Position++;
                        return true;
                    }));
            Transition<SharedState> unparentedValue = new Transition<SharedState>(
                "Unparented value",
                new Condition<SharedState>(
                    "Main",
                    x =>
                    {
                        if (x.CurrentArgument is null)
                        {
                            x.Results.UnparentedValues.Add(
                                Position.Front,
                                x.CurrentValue);
                        }
                        else
                        {
                            x.Results.UnparentedValues.Add(
                                x.CurrentArgument,
                                x.CurrentValue);
                        }

                        x.Position++;
                        return true;
                    }));
            Transition<SharedState> value = new Transition<SharedState>(
                "Value",
                new Condition<SharedState>(
                    "Unparented value",
                    x =>
                    {
                        // If we don't have an argument, or our last argument ran out of values, then this is actually
                        // an unparented value.
                        if (x.CurrentArgument == null)
                        {
                            return true;
                        }
                        else if ((x.Position - x.PositionAtTimeOfLastArgumentSet)
                            > x.CurrentArgument.OperandCount.UpperBound)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }),
                new Condition<SharedState>(
                    "Main",
                    x =>
                    {
                        x.Results.ParentedValues.Add(x.CurrentArgument, x.CurrentValue);
                        x.Position++;
                        return true;
                    }));

            Transition<SharedState> main = new Transition<SharedState>(
                "Main",
                new Condition<SharedState>(
                    "No more values",
                    x =>
                    {
                        return x.Position >= x.Values.Count;
                    }),
                new Condition<SharedState>(
                    "Null or empty",
                    x =>
                    {
                        return string.IsNullOrEmpty(x.CurrentValue);
                    }),
                new Condition<SharedState>(
                    "Starts with hyphen",
                    x =>
                    {
                        return x.CurrentValue[0] == '-';
                    }),
                new Condition<SharedState>(
                    "Value",
                    x =>
                    {
                        return x.CurrentArgument != null;
                    }),
                new Condition<SharedState>(
                    "Unparented value",
                    x =>
                    {
                        return true;
                    }));

            PosixParser.StateMachine = new StateMachine<SharedState>(
                main,
                new List<Transition<SharedState>>()
                {
                    main,
                    nullOrEmpty,
                    startsWithHyphen,
                    value,
                    unparentedValue,
                    notEnoughOperands,
                    noMoreValues,
                    readToEnd,
                });
        }

        public static PosixParser Singleton { get; } = new PosixParser();

        public ParseResult Parse(
            IReadOnlySet<Argument> arguments,
            IReadOnlyList<string> values) => PosixParser
                .StateMachine
                .Run(() => new SharedState(arguments, values))
                .Results
                .ToParseResult();

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
                            "POSIX-style parsing requires that short names be only one character long.");
                    }

                    char character = shortName[0];
                    if (character == '-')
                    {
                        throw new ArgumentException(
                            "POSIX-style parsing requires short names not start with '-'.");
                    }

                    if (!characters.Add(character))
                    {
                        throw new ArgumentException(
                            "POSIX-style parsing requires that short names have a unique first character.");
                    }
                }
            }
        }
    }
}
