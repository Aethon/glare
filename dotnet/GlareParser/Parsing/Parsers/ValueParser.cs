using System.Collections.Generic;
using System.Threading.Tasks;
using Aethon.Glare.Util;

namespace Aethon.Glare.Parsing.Parsers
{
    public sealed class ValueParser<E> : ParserBase<E, E>
    {
        public readonly E Value;

        public ValueParser(E value)
        {
            Value = Preconditions.NotNull(value, nameof(value));
        }

        public override Task<ParseResult<E, E>> Resolve(Input<E> input) =>
            input.Select(
                element => EqualityComparer<E>.Default.Equals(element.Value, Value)
                    ? MatchTask(element.Value, element.Next())
                    : NoMatchTask(element.Position),
                end => NoMatchTask(end.Position)
            );

        public override string Description => $@"""{Value}""";
    }
}