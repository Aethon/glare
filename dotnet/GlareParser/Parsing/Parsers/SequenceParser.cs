using System.Collections.Immutable;
using System.Threading.Tasks;
using Aethon.Glare.Util;

namespace Aethon.Glare.Parsing.Parsers
{
    public sealed class SequenceParser<E, M> : ParserBase<E, ImmutableList<M>>
    {
        public readonly IParser<E, M> ItemParser;
        public readonly ImmutableList<M> Prefix;

        public SequenceParser(IParser<E, M> itemParser, ImmutableList<M> prefix)
        {
            ItemParser = Preconditions.NotNull(itemParser, nameof(itemParser));
            Prefix = prefix;
        }

        public override async Task<ParseResult<E, ImmutableList<M>>> Resolve(Input<E> input)
        {
            return await (await input.Resolve(ItemParser))
                .Bind(async (m, r) =>
                {
                    var list = Prefix.Add(m);
                    return Match(list, r)
                        .And(await r.Resolve(new SequenceParser<E, M>(ItemParser, list)));
                });
        }

        public override string Description => $"sequence of {ItemParser}";
    }
}