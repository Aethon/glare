using System.Collections.Immutable;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Aethon.Glare.Parsing
{
    public static class ParserVerificationExtensions
    {
        public static ParseResult<E, M> Dump<E, M>(this ParseResult<E, M> @this, ITestOutputHelper log)
        {
            log.WriteLine(@this.ToString());
            return @this;
        }

//        public static async Task<ParseResult<char, TMatch>> ParseAndDump<TMatch>(
//            this IParser<char, TMatch> @this,
//            ParsingContext<char> context,
//            ITestOutputHelper log)
//            => (await @this.Resolve(context.Start)).Dump(log);

        public static async Task<ParseResult<TInput, TMatch>> ParseAndDump<TInput, TMatch>(
            this IParser<TInput, TMatch> @this,
            Input<TInput> input,
            ITestOutputHelper log)
            => (await @this.Resolve(input)).Dump(log);
    }

    public static class ParserTestHelpers
    {

        public static Alternative<E, M> Alt<E, M>(M value, Input<E> remainingInput) =>
            new Alternative<E, M>(value, remainingInput);
//
////        public static FailedExpectation FailedExpectation(string type, int position,
////            ImmutableHashSet<FailedExpectation> causes) =>
////            new FailedExpectation(type, position, causes);
////
////        public static FailedExpectation FailedExpectation(string type, int position) =>
////            new FailedExpectation(type, position);
//
        public static ParseResult<E, M> SingleMatch<E, M>(M value,
            Input<E> remainingInput) =>
            new Match<E, M>(ImmutableHashSet.Create(Alt(value, remainingInput)));
//
//        public static ParseResult<E, M> Matches<E, M>(params Alternative<E, M>[] alts) =>
//            new Match<E, M>(ImmutableHashSet.CreateRange(alts));
//
//        public static ParseResult<E, M> Matches<E, M>(ImmutableHashSet<Alternative<E, M>> alts) =>
//            new Match<E, M>(alts);
//
//        public static ParseResult<E, M> Nothing<E, M>(string type, int position) =>
//            new Nothing<E, M>(ImmutableHashSet.Create(FailedExpectation(type, position)));
        
    }
}