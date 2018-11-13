using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Aethon.Glare.Parsing.Parsers
{
    public abstract class ParserBase<E, M> : IParser<E, M>
    {
        public object Key => this;

        public abstract Task<ParseResult<E, M>> Resolve(Input<E> input);

        public abstract string Description { get; }

        public sealed override string ToString() => $"P({Description})";

        protected ParseResult<E, M> NoMatch(int position) =>
            new Nothing<E, M>(ImmutableHashSet.Create(new FailedExpectation(this, position)));

        protected Task<ParseResult<E, M>> NoMatchTask(int position) =>
            Task.FromResult((ParseResult<E, M>)new Nothing<E, M>(ImmutableHashSet.Create(new FailedExpectation(this, position))));

        protected ParseResult<E, M> Match(M value, Input<E> remainingInput) =>
            new Match<E, M>(ImmutableHashSet.Create(new Alternative<E, M>(value, remainingInput)));

        protected Task<ParseResult<E, M>> MatchTask(M value, Input<E> remainingInput) =>
            Task.FromResult((ParseResult<E, M>)new Match<E, M>(ImmutableHashSet.Create(new Alternative<E, M>(value, remainingInput))));
    }
}