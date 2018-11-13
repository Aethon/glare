using System.Threading.Tasks;
using Aethon.Glare.Util;

namespace Aethon.Glare.Parsing.Parsers
{
    public sealed class ReturnParser<E, M> : ParserBase<E, M>
    {
        public readonly M Value;

        public ReturnParser(M value)
        {
            Value = Preconditions.NotNull(value, nameof(value));
        }

        public override Task<ParseResult<E, M>> Resolve(Input<E> input) =>
            MatchTask(Value, input);

        public override string Description => $@"always ""{Value}""";
    }
}