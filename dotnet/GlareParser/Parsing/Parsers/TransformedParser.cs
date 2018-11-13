using System;
using System.Threading.Tasks;
using Aethon.Glare.Util;

namespace Aethon.Glare.Parsing.Parsers
{
    public sealed class TransformedParser<E, M1, M2> : ParserBase<E, M2>
    {
        public readonly IParser<E, M1> ActualParser;
        public readonly Func<M1, M2> Transform;

        public TransformedParser(IParser<E, M1> innerParser, Func<M1, M2> transform)
        {
            ActualParser = Preconditions.NotNull(innerParser, nameof(innerParser));
            Transform = Preconditions.NotNull(transform, nameof(transform));
        }

        public override async Task<ParseResult<E, M2>> Resolve(Input<E> input) =>
            // TODO: move into context?
            await (await input.Resolve(ActualParser)).Bind((m, r) => MatchTask(Transform(m), r));

        public override string Description => ActualParser.ToString();
    }
}