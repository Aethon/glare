using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Aethon.Glare.Parsing
{
    public abstract class ParseResult<TInput, TMatch>
    {

    }

    public sealed class ParseMatch<TInput, TMatch> : ParseResult<TInput, TMatch>
    {
        public readonly ImmutableList<Match<TInput, TMatch>> Alternatives;

        public ParseMatch(ImmutableList<Match<TInput, TMatch>> alternatives)
        {
            Alternatives = alternatives;
        }
    }
    
    public sealed class ParseNothing<TInput, TMatch> : ParseResult<TInput, TMatch>
    {
        public readonly ImmutableList<Nothing<TInput, TMatch>> Expectations;

        public ParseNothing(ImmutableList<Nothing<TInput, TMatch>> expectations)
        {
            Expectations = expectations;
        }
    }
    
    public static class ParseResults
    {
        public static ParseResult<TInput, TMatch> ParseMatch<TInput, TMatch>(TMatch match, Input<TInput> remainingInput) =>
            new ParseMatch<TInput, TMatch>(ImmutableList.Create(new Match<TInput, TMatch>(match, remainingInput)));

//        public static ParseResult<TInput, TMatch> ParseNothing<TInput, TMatch>(Nothing<TInput, TMatch> nothing) =>
//            new ParseNothing<TInput, TMatch>(ImmutableList.Create(new Failure<TInput, TMatch>(expectation, position)));

//        public static ParseResult<TInput, TMatch> ParseResult<TInput, TMatch>(
//            params Task<Resolution<TInput, TMatch>>[] alternatives) =>
//            new ParseResult<TInput, TMatch>(ImmutableList.Create(alternatives));

//        public static ParseResult<TInput, TMatch> ParseResult<TInput, TMatch>(
//            ImmutableList<Task<Resolution<TInput, TMatch>>> alternatives) =>
//            new ParseResult<TInput, TMatch>(alternatives);
    }
}