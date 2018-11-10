using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aethon.Glare.Parsing
{
    public abstract class ParseResult<TInput, TMatch>
    {
//        public abstract Task<T> Select<T>(Func<TMatch, Input<TInput>, Task<T>> match = null,
//            Func<ImmutableList<FailedExpectation>, Task<T>> nothing = null);

        public abstract ParseResult<TInput, TMatch> And(ParseResult<TInput, TMatch> other);
    }

    public sealed class Match<TInput, TMatch> : ParseResult<TInput, TMatch>
    {
        public readonly ImmutableList<Alternative<TInput, TMatch>> Alternatives;

        public Match(ImmutableList<Alternative<TInput, TMatch>> alternatives)
        {
            Alternatives = alternatives;
        }

//        public override async Task<T> Select<T>(Func<TMatch, Input<TInput>, Task<T>> match = null,
//            Func<ImmutableList<FailedExpectation>, Task<T>> nothing = null)
//        {
//            if (match == null) return default;
//
//            var tasks = Alternatives.Select(alt => match(alt.Value, alt.RemainingInput));
//            var results = (await Task.WhenAll(tasks)).Aggregate((a, r) => a.And(r));
//            var matches = new List<Alternative<TInput, T>>();
//            var nothings = new StringBuilder();
//            foreach (var result in results)
//            {
//                result.Apply(
//                    match: a => matches.AddRange(a),
//                    nothing: (ex, pos) => nothings.Append($"@{pos}: {ex}\n")
//                );
//            }
//
//            if (matches.Count > 0)
//                return new Match<TInput, TMatch2>(matches.ToImmutableList());
//            return new Nothing<TInput, TMatch2>(nothings.ToString(), 0); // TODO: position
//        }

        public override ParseResult<TInput, TMatch> And(ParseResult<TInput, TMatch> other)
        {
            switch (other)
            {
                case Match<TInput, TMatch> match:
                    return new Match<TInput, TMatch>(Alternatives.AddRange(match.Alternatives));
                default:
                    return this;
            }
        }
    }

    public struct Alternative<TInput, TMatch>
    {
        /// <summary>
        /// Matched value
        /// </summary>
        public readonly TMatch Value;

        public readonly Input<TInput> RemainingInput;

        /// <summary>
        /// Creates a new Match with no trivia
        /// </summary>
        /// <param name="value">Matched value</param>
        public Alternative(TMatch value, Input<TInput> remainingInput)
        {
            Value = value;
            RemainingInput = remainingInput;
        }
    }

    /// <summary>
    /// Represents a failed parse match
    /// </summary>
    /// <typeparam name="TMatch">Parser match type</typeparam>
    public sealed class Nothing<TInput, TMatch> : ParseResult<TInput, TMatch>
    {
        public readonly ImmutableList<FailedExpectation> Expectations;

        public Nothing(ImmutableList<FailedExpectation> expectations)
        {
            Expectations = expectations;
        }

        public Nothing<TInput, TMatch2> As<TMatch2>() =>
            new Nothing<TInput, TMatch2>(Expectations);

//        public override Task<T> Select<T>(Func<TMatch, Input<TInput>, Task<T>> match = null,
//            Func<ImmutableList<FailedExpectation>, Task<T>> nothing = null)
//        {
//            return nothing == null ? Task.FromResult(default(T)) : nothing(Expectations);
//        }

        public override ParseResult<TInput, TMatch> And(ParseResult<TInput, TMatch> other)
        {
            switch (other)
            {
                case Match<TInput, TMatch> match:
                    return match;
                case Nothing<TInput, TMatch> nothing:
                    return new Nothing<TInput, TMatch>(Expectations.AddRange(nothing.Expectations));
                default:
                    throw new Exception(); // TODO
            }
        }
    }

    public sealed class FailedExpectation
    {
        public readonly string Type;
        public readonly int Position;
        public readonly ImmutableList<FailedExpectation> Causes;

        public FailedExpectation(string type, int position) : this(type, position, ImmutableList<FailedExpectation>.Empty)
        {
        }

        public FailedExpectation(string type, int position, ImmutableList<FailedExpectation> causes)
        {
            Type = type;
            Position = position;
            Causes = causes;
        }
    }
    
    public static class ParseResults
    {
        public static ParseResult<TInput, TMatch>
            ParseMatch<TInput, TMatch>(TMatch match, Input<TInput> remainingInput) =>
            new Match<TInput, TMatch>(ImmutableList.Create(new Alternative<TInput, TMatch>(match, remainingInput)));

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