using System.Collections.Generic;

namespace Drexel.Arguments.Parsers.Internals
{
    internal class MutableParseResult
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
}
