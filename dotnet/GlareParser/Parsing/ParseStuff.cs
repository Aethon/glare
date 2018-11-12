using System.Collections.Immutable;
using System.Net;

namespace Aethon.Glare.Parsing
{
    public static class ParseStuff
    {
        public static Alternative<E, M> Alt<E, M>(M value, Input<E> remainingInput) =>
            new Alternative<E, M>(value, remainingInput);

        public static FailedExpectation FailedExpectation(string type, int position,
            ImmutableHashSet<FailedExpectation> causes) =>
            new FailedExpectation(type, position, causes);

        public static FailedExpectation FailedExpectation(string type, int position) =>
            new FailedExpectation(type, position);

        public static ParseResult<E, M> SingleMatch<E, M>(M value,
            Input<E> remainingInput) =>
            new Match<E, M>(ImmutableHashSet.Create(Alt(value, remainingInput)));

        public static ParseResult<E, M> Matches<E, M>(params Alternative<E, M>[] alts) =>
            new Match<E, M>(ImmutableHashSet.CreateRange(alts));

        public static ParseResult<E, M> Matches<E, M>(ImmutableHashSet<Alternative<E, M>> alts) =>
            new Match<E, M>(alts);

        public static ParseResult<E, M> Nothing<E, M>(string type, int position) =>
            new Nothing<E, M>(ImmutableHashSet.Create(FailedExpectation(type, position)));
    }
}