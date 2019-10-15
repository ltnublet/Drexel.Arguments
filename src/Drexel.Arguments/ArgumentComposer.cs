using System;
using System.Collections.Generic;

namespace Drexel.Arguments
{
    public class ArgumentComposer
    {
        private readonly ArgumentParser parser;
        private readonly IReadOnlyDictionary<Argument, CountBounds> argumentCountLimits;
        private readonly Argument? helpArgument;
        private readonly Argument? versionArgument;

        public ArgumentComposer(
            ArgumentParser parser,
            IReadOnlyDictionary<Argument, CountBounds> argumentCountLimits,
            Argument? helpArgument = null,
            Argument? versionArgument = null)
        {
            this.parser =
                parser ?? throw new ArgumentNullException(nameof(parser));
            this.argumentCountLimits =
                argumentCountLimits ?? throw new ArgumentNullException(nameof(argumentCountLimits));

            this.helpArgument = helpArgument;
            if (this.helpArgument != null)
            {
                if (!this.parser.Arguments.Contains(helpArgument))
                {
                    throw new ArgumentException(
                        "Help argument must be contained by parser if specified.",
                        nameof(helpArgument));
                }
            }

            this.versionArgument = versionArgument;
            if (this.versionArgument != null)
            {
                if (!this.parser.Arguments.Contains(versionArgument))
                {
                    throw new ArgumentException(
                        "Version argument must be contained by parser if specified.",
                        nameof(versionArgument));
                }
            }
        }

        public ParseResult Compose(
            IReadOnlyList<string> values,
            IReadOnlyDictionary<Argument, ArgumentCallbacks>? callbacks,
            IReadOnlyList<object> objects)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (objects is null)
            {
                throw new ArgumentNullException(nameof(objects));
            }

            ParseResult result = this.parser.Parse(values);
            foreach (KeyValuePair<Argument, CountBounds> kvp in this.argumentCountLimits)
            {
                if (result.ParentedValues.TryGetValue(kvp.Key, out IReadOnlyList<IReadOnlyList<string>> appearances))
                {
                    if (appearances.Count < kvp.Value.LowerBound)
                    {
                        throw new InvalidOperationException(
                            $"Argument '{kvp.Key.HumanReadableName}' was supplied {appearances.Count} time(s), but it was required to appear at least {kvp.Value.LowerBound} time(s).");
                    }
                    else if (kvp.Value.UpperBound.HasValue && appearances.Count > kvp.Value.UpperBound.Value)
                    {
                        throw new InvalidOperationException(
                            $"Argument '{kvp.Key.HumanReadableName}' was supplied {appearances.Count} time(s), but it was required to appear no more than {kvp.Value.UpperBound.Value} time(s).");
                    }
                }
            }

            if (callbacks != null)
            {
                foreach (KeyValuePair<Argument, ArgumentCallbacks> kvp in callbacks)
                {
                    if (result.ParentedValues.ContainsKey(kvp.Key))
                    {
                        kvp.Value.OnSupplied?.Invoke(result);
                    }
                    else
                    {
                        kvp.Value.OnOmitted?.Invoke(result);
                    }
                }
            }

            Dictionary<Type, Action<object, ParseResult>> precomputed =
                new Dictionary<Type, Action<object, ParseResult>>();
            foreach (object obj in objects)
            {
                this.TryCompose(obj, result, in precomputed);
            }

            return result;
        }

        private void TryCompose(
            object obj,
            ParseResult result,
            in Dictionary<Type, Action<object, ParseResult>> precomputed)
        {

        }
    }
}
