using System.Threading.Tasks;
using static Aethon.Glare.Util.Preconditions;

namespace Aethon.Glare.Parsing.Parsers
{
    public abstract class DecoratedParser<E, M> : ParserBase<E, M>
    {
        // Actual parser to be used
        protected readonly IParser<E, M> ActualParser;

       protected DecoratedParser(IParser<E, M> actualParser)
        {
            ActualParser = NotNull(actualParser, nameof(actualParser));
        }

        /// <inheritdoc/>
        public override Task<ParseResult<E, M>> Resolve(Input<E> input) => ActualParser.Resolve(input);

        /// <inheritdoc/>
        public override string Description => ActualParser.Description;
    }
}