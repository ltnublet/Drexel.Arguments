using System;
using System.Collections.Generic;
using Drexel.Arguments.Parsers.Internals;
using Drexel.Collections.Generic;

namespace Drexel.Arguments.Parsers.Implementations
{
    internal sealed class MsBuildParser : IParser
    {
        private static readonly StateMachine<SharedState> StateMachine;

        static MsBuildParser()
        {
            Argument CheckArgument(
                IReadOnlySet<Argument> arguments,
                string currentValue)
            {
                Argument? recognized = null;
                foreach (Argument argument in arguments)
                {
                    if (argument.ShortNames.Contains(currentValue))
                    {
                        recognized = argument;
                        break;
                    }
                    else if (argument.LongNames.Contains(currentValue))
                    {
                        recognized = argument;
                        break;
                    }
                }

                if (recognized is null)
                {
                    throw new InvalidOperationException(
                        $"Unrecognized argument: {currentValue}");
                }

                return recognized;
            }

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
                        && x.ValuesAddedToCurrentArgument < x.CurrentArgument.OperandCount.LowerBound),
                new Condition<SharedState>(
                    "Main",
                    x =>
                    {
                        return ConditionResult.Stop;
                    }));
            Transition<SharedState> argumentContainsColon = new Transition<SharedState>(
                "Argument contained colon",
                new Condition<SharedState>(
                    "Unparented value",
                    x =>
                    {
                        // TODO: This means they did something like `/flag:Value`. This seems illegal to me, but if
                        // we returned true instead of throwing, we could treat `Value` as unparented instead.
                        if (x.CurrentArgument != null && x.CurrentArgument.OperandCount == CountBounds.Flag)
                        {
                            throw new InvalidOperationException(
                                "Flag argument cannot have attached value.");
                        }

                        return false;
                    }),
                new Condition<SharedState>(
                    "Value",
                    x =>
                    {
                        x.CurrentValue = x.CurrentValue.Substring(x.CurrentValue.IndexOf(':') + 1);
                        return true;
                    }));
            Transition<SharedState> setCurrentArgument = new Transition<SharedState>(
                "Set current argument",
                new Condition<SharedState>(
                    "Main",
                    x =>
                    {
                        Argument newArgument = CheckArgument(x.Arguments, x.CurrentValue);
                        x.Results.Order.Add(newArgument);

                        x.CurrentArgument = newArgument;
                        x.Position++;
                        return true;
                    }));
            Transition<SharedState> setCurrentArgumentWithColon = new Transition<SharedState>(
                "Set current argument with colon",
                new Condition<SharedState>(
                    "Argument contained colon",
                    x =>
                    {
                        Argument newArgument = CheckArgument(
                            x.Arguments,
                            x.CurrentValue.Substring(0, x.CurrentValue.IndexOf(':')));

                        x.Results.Order.Add(newArgument);
                        x.CurrentArgument = newArgument;
                        return true;
                    }));
            Transition<SharedState> startsWithSlashOrHyphen = new Transition<SharedState>(
                "Starts with slash or hyphen",
                new Condition<SharedState>(
                    "Value",
                    x =>
                    {
                        // If they just sent us "/" or '-', then that's actually a value.
                        string substring = x.CurrentValue.Substring(1);
                        if (substring.Length == 0)
                        {
                            return true;
                        }
                        else
                        {
                            // Remove the '/' or '-' from the front so that future steps don't need to re-do the work
                            x.CurrentValue = substring;
                        }

                        return false;
                    }),
                new Condition<SharedState>(
                    "Set current argument with colon",
                    x =>
                    {
                        return x.CurrentValue.IndexOf(':') >= 0;
                    }),
                new Condition<SharedState>(
                    "Not enough operands",
                    x =>
                    {
                        bool notEnoughOperands = x.CurrentArgument != null
                            && x.ValuesAddedToCurrentArgument < x.CurrentArgument.OperandCount.LowerBound;
                        return notEnoughOperands;
                    }),
                new Condition<SharedState>(
                    "Set current argument",
                    x => true));
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
                        else if (x.ValuesAddedToCurrentArgument >= x.CurrentArgument.OperandCount.UpperBound)
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
                        x.ValuesAddedToCurrentArgument++;
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
                    "Starts with slash or hyphen",
                    x =>
                    {
                        char firstCharacter = x.CurrentValue[0];
                        return firstCharacter == '/' || firstCharacter == '-';
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

            MsBuildParser.StateMachine = new StateMachine<SharedState>(
                main,
                new List<Transition<SharedState>>()
                {
                    main,
                    nullOrEmpty,
                    startsWithSlashOrHyphen,
                    value,
                    unparentedValue,
                    notEnoughOperands,
                    noMoreValues,
                    argumentContainsColon,
                    setCurrentArgument,
                    setCurrentArgumentWithColon
                });
        }

        private MsBuildParser()
        {
        }

        public static MsBuildParser Singleton { get; } = new MsBuildParser();

        public ParseResult Parse(
            IReadOnlySet<Argument> arguments,
            IReadOnlyList<string> values) => MsBuildParser
                .StateMachine
                .Run(() => new SharedState(arguments, values))
                .Results
                .ToParseResult();

        public void ThrowIfIllegal(IReadOnlySet<Argument> arguments)
        {
            HashSet<string> names = new HashSet<string>();
            foreach (Argument argument in arguments)
            {
                bool hasShortName = argument.ShortNames.Count > 0;
                if (hasShortName)
                {
                    foreach (string shortName in argument.ShortNames)
                    {
                        char character = shortName[0];
                        if (character == '/')
                        {
                            throw new ArgumentException(
                                "MSBuild-style parsing requires names not start with '/'.");
                        }
                        else if (character == '-')
                        {
                            throw new ArgumentException(
                                "MSBuild-style parsing requires names not start with '-'.");
                        }

                        if (shortName.Contains(":"))
                        {
                            throw new ArgumentException(
                                "MSBuild-style parsing requires names not contain ':'.");
                        }

                        if (!names.Add(shortName))
                        {
                            throw new ArgumentException(
                                "MSBuild-style parsing requires that short and long names be unique, and have no overlap.");
                        }
                    }
                }

                bool hasLongName = argument.LongNames.Count > 0;
                if (hasLongName)
                {
                    foreach (string longName in argument.LongNames)
                    {
                        char character = longName[0];
                        if (character == '/')
                        {
                            throw new ArgumentException(
                                "MSBuild-style parsing requires names not start with '/'.");
                        }
                        else if (character == '-')
                        {
                            throw new ArgumentException(
                                "MSBuild-style parsing requires names not start with '-'.");
                        }

                        if (longName.Contains(":"))
                        {
                            throw new ArgumentException(
                                "MSBuild-style parsing requires names not contain ':'.");
                        }

                        if (!names.Add(longName))
                        {
                            throw new ArgumentException(
                                "MSBuild-style parsing requires that short and long names be unique, and have no overlap.");
                        }
                    }
                }

                if (!hasShortName && !hasLongName)
                {
                    throw new ArgumentException(
                        "MSBuild-style parsing requires that arguments have at least one short or long name.");
                }
            }
        }
    }
}
