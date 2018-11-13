using System.Collections.Immutable;
using Xunit.Abstractions;

namespace Aethon.Glare.Parsing
{
    public abstract class ParsingUnitTest<E>
    {
        protected ITestOutputHelper Out { get; }

        protected ParsingUnitTest(ITestOutputHelper output)
        {
            Out = output;
        }
        
        public static Alternative<E, M> Alt<M>(M value, Input<E> remainingInput) =>
            new Alternative<E, M>(value, remainingInput);
//
////        public static FailedExpectation FailedExpectation(string type, int position,
////            ImmutableHashSet<FailedExpectation> causes) =>
////            new FailedExpectation(type, position, causes);
////
////        public static FailedExpectation FailedExpectation(string type, int position) =>
////            new FailedExpectation(type, position);
//
        public static ParseResult<E, M> SingleMatch<M>(M value, Input<E> remainingInput) =>
            new Match<E, M>(ImmutableHashSet.Create(Alt(value, remainingInput)));

        public static ParseResult<E, M> Matches<M>(params Alternative<E, M>[] alts) =>
            new Match<E, M>(ImmutableHashSet.CreateRange(alts));

//        public static ParseResult<E, M> Matches<E, M>(ImmutableHashSet<Alternative<E, M>> alts) =>
//            new Match<E, M>(alts);
//
        public static ParseResult<E, M> Nothing<M>(IExpectation expectation, int position) =>
            new Nothing<E, M>(ImmutableHashSet.Create(new FailedExpectation(expectation, position)));
    }
}