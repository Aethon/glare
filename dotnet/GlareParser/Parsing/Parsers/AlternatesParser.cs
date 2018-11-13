using System.Collections.Immutable;
using System.Threading.Tasks;
using Aethon.Glare.Util;

namespace Aethon.Glare.Parsing.Parsers
{
    public sealed class AlternatesParser<E, M> : ParserBase<E, M>
    {
        public readonly ImmutableList<IParser<E, M>> ItemParsers;

        public AlternatesParser(params IParser<E, M>[] itemParsers)
        {
            Preconditions.NotNullOrEmpty(itemParsers, nameof(itemParsers));
            ItemParsers = ImmutableList.CreateRange(itemParsers);
        }

        public override Task<ParseResult<E, M>> Resolve(Input<E> input) => input.Resolve(ItemParsers);

        public override string Description => $"one of {{{string.Join(", ", ItemParsers)}}}";
    }
}