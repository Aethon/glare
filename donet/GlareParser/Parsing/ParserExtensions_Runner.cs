using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Aethon.Glare.Parsing.ParseTree;
using static Aethon.Glare.Parsing.ParseStateExtensions;
using static Aethon.Glare.Util.Preconditions;

namespace Aethon.Glare.Parsing
{
    public static partial class ParserExtensions
    {
        private static readonly Action<string> NoLog = t => { };

        /// <summary>
        /// Runs a parser against an input stream, returning all matches discovered, including those that do not
        /// consume the entire input stream. Identical to <see cref="Parse{T}(Aethon.Glare.Parsing.IParser{T},System.Collections.Generic.IEnumerable{T},Action{string})"/>
        /// but with no log output.
        /// </summary>
        public static IEnumerable<ParseNode> Parse<T>(this IParser<T> @this, IEnumerable<T> input) =>
            Parse(@this, input, NoLog);

        /// <summary>
        /// Runs a parser against an input stream, returning all matches discovered, including those that do not
        /// consume the entire input stream.
        /// </summary>
        /// <param name="this">Parser to run</param>
        /// <param name="input">Input stream to apply</param>
        /// <param name="log">Action that will receive log info</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>
        /// All matches found starting at the beginning of the input, even those that do not consume
        /// the entire input stream.
        /// </returns>
        public static IEnumerable<ParseNode> Parse<T>(this IParser<T> @this, IEnumerable<T> input, Action<string> log)
        {
            NotNull(@this, nameof(@this));
            NotNull((object)input, nameof(input)); // cast tells inspections that we are not enumerating it here
            NotNull(log, nameof(log));
            
            var workList = WorkListExtensions.Work(@this, State<T>);
            using (var enumerator = input.GetEnumerator())
            {
                while (!workList.IsEmpty() && enumerator.MoveNext())
                {
                    var result = Apply(workList, enumerator.Current, log);
                    foreach (var match in result.Matches)
                        yield return match;
                    workList = result.WorkList;
                }
            }
        }
        
        /// <summary>
        /// Runs a parser against an input stream, return all matches that consume the entire input stream.
        /// Identical to <see cref="ParseAll{T}(Aethon.Glare.Parsing.IParser{T},System.Collections.Generic.IEnumerable{T},Action{string})"/>
        /// but with no log output.
        /// </summary>
        public static IEnumerable<ParseNode> ParseAll<T>(this IParser<T> @this, IEnumerable<T> input) =>
            ParseAll(@this, input, NoLog);

        /// <summary>
        /// Runs a parser against an input stream, return all matches that consume the entire input stream.
        /// </summary>
        /// <param name="this">Parser to run</param>
        /// <param name="input">Input stream to apply</param>
        /// <param name="log">Action that will receive log info</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>
        /// All matches found that consume the entire input stream.
        /// </returns>
        public static IEnumerable<ParseNode> ParseAll<T>(this IParser<T> @this, IEnumerable<T> input, Action<string> log)
        {
            NotNull(@this, nameof(@this));
            NotNull((object)input, nameof(input)); // cast tells inspections that we are not enumerating it here
            NotNull(log, nameof(log));

            var workList = WorkListExtensions.Work(@this, State<T>);
            using (var enumerator = input.GetEnumerator())
            {
                var results = ImmutableList<ParseNode>.Empty;
                bool moved;
                while ((moved = enumerator.MoveNext()) && !workList.IsEmpty())
                    (results, workList) = Apply(workList, enumerator.Current, log);
                return moved
                    ? ImmutableList<ParseNode>.Empty
                    : results;
            }
        }

        /// <summary>
        /// This is the "trampoline" function
        /// </summary>
        /// <param name="workList"></param>
        /// <param name="input"></param>
        /// <param name="log"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static ParseState<T> Apply<T>(WorkList<T> workList, T input, Action<string> log)
        {
            log($"Input: '{input}'");
            var (matchers, initialParsers) = workList;
            var parserMap = new Dictionary<IParser<T>, List<Resolver<T>>>();
            var parsers = new Queue<ParserRegistration<T>>(initialParsers);

            log("Creating new work");
            while (parsers.Count > 0)
            {
                var (parser, finalizer) = parsers.Dequeue();
                if (!parserMap.TryGetValue(parser, out var finalizers))
                {
                    log($"Found new parser: {parser}");
                    finalizers = new List<Resolver<T>>();
                    parserMap.Add(parser, finalizers);
                    var (newMatchers, newParsers) = parser.Start(match =>
                        finalizers.Select(f =>
                        {
                            log($"Finalizing match from {parser}");
                            return f(match);
                        }).Aggregate((a, b) => a.Add(b)));
                    matchers = matchers.AddRange(newMatchers);
                    foreach (var reg in newParsers)
                        parsers.Enqueue(reg);
                }
                else
                {
                    log($"Found parser match: {parser}");
                }

                finalizers.Add(finalizer);
            }

            log("Apply input to work");
            return matchers.Select(matcher => matcher(input)).Aggregate((a, b) => a.Add(b));
        }
    }
}