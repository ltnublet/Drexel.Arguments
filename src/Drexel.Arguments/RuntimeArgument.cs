using System;
using System.Collections.Generic;
using System.Diagnostics;
using Drexel.Collections.Generic;

namespace Drexel.Arguments
{
    [DebuggerDisplay("{HumanReadableName,nq}")]
    public class RuntimeArgument : Argument
    {
        public RuntimeArgument(
            string humanReadableName,
            IReadOnlyInvariantSet<string> shortNames,
            IReadOnlyInvariantSet<string> longNames,
            string description,
            CountBounds? operandCount = null)
            : base(
                  humanReadableName,
                  shortNames,
                  longNames,
                  description,
                  operandCount ?? CountBounds.Single)
        {
        }

        public RuntimeArgument(
            string humanReadableName,
            IReadOnlyList<string> shortNames,
            IReadOnlyList<string> longNames,
            string description,
            CountBounds? operandCount = null)
            : this(
                  humanReadableName,
                  RuntimeArgument.ToSet(shortNames, nameof(shortNames)),
                  RuntimeArgument.ToSet(longNames, nameof(longNames)),
                  description,
                  operandCount)
        {
        }

        private static IReadOnlyInvariantSet<string> ToSet(
            IReadOnlyList<string> names,
            string parameterName)
        {
            if (names == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            SetAdapter<string> adapter = new SetAdapter<string>(new HashSet<string>());
            foreach (string name in names)
            {
                if (!adapter.Add(name))
                {
                    throw new ArgumentException("Duplicate name.", parameterName);
                }
            }

            return adapter;
        }
    }
}
