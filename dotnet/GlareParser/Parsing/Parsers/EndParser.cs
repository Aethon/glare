using System.Threading.Tasks;

namespace Aethon.Glare.Parsing.Parsers
{
    public sealed class EndParser<E> : ParserBase<E, NoValue>
    {
        public static readonly EndParser<E> Singleton = new EndParser<E>();

        private EndParser()
        {
        }

        public override Task<ParseResult<E, NoValue>> Resolve(Input<E> input) =>
            input.Select(
                element => NoMatchTask(element.Position),
                end => MatchTask(NoValue.Instance, end)
            );

        public override string Description => "end of the source";
    }
}