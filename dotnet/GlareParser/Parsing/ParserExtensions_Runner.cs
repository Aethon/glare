using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using static Aethon.Glare.Parsing.WorkListExtensions;
using static Aethon.Glare.Util.Preconditions;

namespace Aethon.Glare.Parsing
{
    public static partial class ParserExtensions
    {
        private static readonly Action<string> NoLog = t => { };

        /// <summary>
        /// Runs a parser against an input stream, returning all matches discovered, including those that do not
        /// consume the entire input stream.
        /// </summary>
        /// <param name="this">Parser to run</param>
        /// <param name="input">Input stream to apply</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TMatch">Parse result type</typeparam>
        /// <returns>
        /// All matches found starting at the beginning of the input, even those that do not consume
        /// the entire input stream.
        /// </returns>
        public static IEnumerable<TMatch> Parse<TInput, TMatch>(this IParser<TInput, TMatch> @this, IEnumerable<TInput> input) =>
            Parse(@this, input, NoLog);

        /// <summary>
        /// Runs a parser against an input stream, returning all matches discovered, including those that do not
        /// consume the entire input stream.
        /// </summary>
        /// <param name="this">Parser to run</param>
        /// <param name="input">Input stream to apply</param>
        /// <param name="log">Action that will receive log info</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TMatch">Parse result type</typeparam>
        /// <returns>
        /// All matches found starting at the beginning of the input, even those that do not consume
        /// the entire input stream.
        /// </returns>
        public static IEnumerable<TMatch> Parse<TInput, TMatch>(this IParser<TInput, TMatch> @this, IEnumerable<TInput> input, Action<string> log)
        {
            NotNull(@this, nameof(@this));
            NotNull((object)input, nameof(input)); // cast tells inspections that we are not enumerating it here
            NotNull(log, nameof(log));
            
            using (var enumerator = input.GetEnumerator())
            {
                var results = new List<TMatch>();
                var failures = new List<Failure<TMatch>>();
                var workList = @this.Start(resolution => {
                    switch (resolution)
                    {
                        case Match<TMatch> match:
                            results.Add(match.Value);
                            break;
                        case Failure<TMatch> failure:
                            failures.Add(failure);
                            break;
                        default:
                            throw new Exception(); // TODO:
                    }
                    return WorkList<TInput>.Nothing;
                });
                
                while (true)
                {
                    foreach (var match in results)
                        yield return match;
                    results.Clear();
                    if (workList.IsEmpty() || !enumerator.MoveNext())
                        break;
                    workList = Apply(workList, enumerator.Current, log);
                }
            }
        }
        
        /// <summary>
        /// Runs a parser against an input stream, return all matches that consume the entire input stream.
        /// </summary>
        /// <param name="this">Parser to run</param>
        /// <param name="input">Input stream to apply</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TMatch">Parse result type</typeparam>
        /// <returns>
        /// All matches found that consume the entire input stream.
        /// </returns>
        public static IEnumerable<TMatch> ParseAll<TInput, TMatch>(this IParser<TInput, TMatch> @this, IEnumerable<TInput> input) =>
            ParseAll(@this, input, NoLog);

        /// <summary>
        /// Runs a parser against an input stream, return all matches that consume the entire input stream.
        /// </summary>
        /// <param name="this">Parser to run</param>
        /// <param name="input">Input stream to apply</param>
        /// <param name="log">Action that will receive log info</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TMatch">Parse result type</typeparam>
        /// <returns>
        /// All matches found that consume the entire input stream.
        /// </returns>
        public static IEnumerable<TMatch> ParseAll<TInput, TMatch>(this IParser<TInput, TMatch> @this, IEnumerable<TInput> input, Action<string> log)
        {
            NotNull(@this, nameof(@this));
            NotNull((object)input, nameof(input)); // cast tells inspections that we are not enumerating it here
            NotNull(log, nameof(log));

            var results = new List<TMatch>();
            var failures = new List<Failure<TMatch>>();
            var workList = @this.Start(resolution => {
                switch (resolution)
                {
                    case Match<TMatch> match:
                        results.Add(match.Value);
                        break;
                    case Failure<TMatch> failure:
                        failures.Add(failure);
                        break;
                    default:
                        throw new Exception(); // TODO:
                }
                return WorkList<TInput>.Nothing;
            });
            using (var enumerator = input.GetEnumerator())
            {
                bool moved;
                while ((moved = enumerator.MoveNext()) && !workList.IsEmpty())
                {
                    results.Clear();
                    workList = Apply(workList, enumerator.Current, log);
                }

                return moved
                    ? ImmutableList<TMatch>.Empty
                    : results.ToImmutableList();
            }
        }

        /// <summary>
        /// This is the "trampoline" function
        /// </summary>
        /// <param name="workList"></param>
        /// <param name="input"></param>
        /// <param name="log"></param>
        /// <typeparam name="TInput"></typeparam>
        /// <returns></returns>
        private static WorkList<TInput> Apply<TInput>(WorkList<TInput> workList, TInput input, Action<string> log)
        {
            log($"Input: '{input}'");
            var (matchers, initialParsers) = workList;
            var parsers = new Queue<RegisterParser<TInput>>(initialParsers);

            var registrar = new ParserRegistrar<TInput>();
            log("Creating new work");
            while (parsers.Count > 0)
            {
                foreach (var registration in parsers.Dequeue()(registrar))
                    parsers.Enqueue(registration);
            }

            matchers = matchers.AddRange(registrar.GetWork());
            log("Apply input to work");
            return matchers.Select(matcher => matcher(input)).Aggregate((a, b) => a.Add(b));
        }
    }
}