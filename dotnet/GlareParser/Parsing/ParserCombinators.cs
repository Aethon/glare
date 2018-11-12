using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.XPath;
using Aethon.Glare.Util;
using static Aethon.Glare.Parsing.ParserExtensions;
using static Aethon.Glare.Parsing.ParseStuff;

namespace Aethon.Glare.Parsing
{
    /// <summary>
    /// Factories for creating general Glare parsers.
    /// </summary>
    public static class ParserCombinators
    {
//        /// <summary>
//        /// Creates a parser that applies another parser and also matches nothing.
//        /// </summary>
//        /// <param name="parser">Parser to match</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TMatch">Parser match type</typeparam>
//        /// <returns>The new parser</returns>
        public static BasicParser<E, Maybe<M>> Optional<E, M>(IParser<E, M> parser)
        {
            var innerParser = parser.Bind(r => Parsers<E>.Return(new Maybe<M>(r)));
            return Parser<E, Maybe<M>>(async input => SingleMatch(Maybe<M>.Empty, input)
                    .And(await input.Resolve(innerParser)))
                .WithDescription($"({parser})?");
        }

//        /// <summary>
//        /// Creates a parser that starts many parsers in parallel.
//        /// </summary>
//        /// <param name="options">Parsers to start (in parallel)</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TMatch">Parser match type</typeparam>
//        /// <returns>The new parser</returns>
        public static BasicParser<E, M> OneOf<E, M>(params IParser<E, M>[] options)
        {
            Preconditions.NotNullOrEmpty(options, nameof(options));
            return Parser<E, M>(
                    async input =>
                        (await Task.WhenAll(options.Select(input.Resolve))).Aggregate((a,m) => a.And(m))
                )
                .WithDescription($"({string.Join<IParser<E, M>>(" | ", options)})");
        }
//
//        /// <summary>
//        /// Creates a parser that starts a parser repeatedly, zero or more times.
//        /// </summary>
//        /// <param name="item">Parser to start</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TMatch">Parser match type</typeparam>
//        /// <returns>The new parser</returns>
//        public static BasicParser<TInput, ImmutableList<TMatch>> ZeroOrMore<TInput, TMatch>(
//            IParser<TInput, TMatch> item)
//        {
//            var oneOrMore = OneOrMore(item);
//            return ParserExtensions.Parser<TInput, ImmutableList<TMatch>>(
//                    (input, continuation) =>
//                    {
//                        continuation(ImmutableList<TMatch>.Empty);
//                        input.Resolve(oneOrMore, continuation);
//                        return Task.FromResult(true);
//                    })
//                .WithDescription($"({item})*");
//        }
//
//        /// <summary>
//        /// Creates a parser that starts a parser repeatedly, one or more times.
//        /// </summary>
//        /// <param name="item">Parser to start</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TMatch">Parser match type</typeparam>
//        /// <returns>The new parser</returns>
        public static BasicParser<E, ImmutableList<M>> OneOrMore<E, M>(IParser<E, M> item)
        {
            Preconditions.NotNull(item, nameof(item));
            return List(item, ImmutableList<M>.Empty);
        }

        private static BasicParser<E, ImmutableList<M>> List<E, M>(IParser<E, M> item,
            ImmutableList<M> previous)
        {
            return Parser<E, ImmutableList<M>>(async input =>
                    {
                        var result = await input.Resolve(item);
                        switch (result)
                        {
                            case Match<E, M> match:
                                var newAlts = match.Alternatives
                                    .Select(a => Alt(previous.Add(a.Value), a.RemainingInput)).ToImmutableHashSet();
                                var newTasks = newAlts.Select(a => a.RemainingInput.Resolve(List(item, a.Value)));
                                var additionalResults = await Task.WhenAll(newTasks);
                                return additionalResults.Aggregate(Matches(newAlts), (a, m) => a.And(m));
                            case Nothing<E, M> nothing:
                                return nothing.As<ImmutableList<M>>();
                            default:
                                throw new Exception(); // TODO:
                        }
                    }
                )
                .WithDescription($"({item})+");
        }

//
//        /// <summary>
//        /// Creates a parser that matches one or more items interspersed with a separator.
//        /// </summary>
//        /// <param name="item">Parser to match items</param>
//        /// <param name="separator">Parser to match separators</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TMatch">Parser match type</typeparam>
//        /// <typeparam name="TSeparator">Separator parser match type</typeparam>
//        /// <returns>The new parser</returns>
//        public static BasicParser<TInput, ImmutableList<TMatch>> SeparatedList<TInput, TMatch, TSeparator>(
//            IParser<TInput, TMatch> item, IParser<TInput, TSeparator> separator)
//        {
//            var listParser = NonEmptySeparatedList(item, separator);
//            return ParserExtensions.Parser<TInput, ImmutableList<TMatch>>(resolve =>
//                WorkListExtensions.Add(resolve(new Match<TInput, ImmutableList<TMatch>>(ImmutableList<TMatch>.Empty)),
//                    listParser, resolve));
//        }
//
//        /// <summary>
//        /// Creates a parser that any number of items interspersed with a separator.
//        /// </summary>
//        /// <param name="item">Parser to match items</param>
//        /// <param name="separator">Parser to match separators</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TMatch">Parser match type</typeparam>
//        /// <typeparam name="TSeparator">Separator parser match type</typeparam>
//        /// <returns>The new parser</returns>
//        public static BasicParser<TInput, ImmutableList<TMatch>> NonEmptySeparatedList<TInput, TMatch, TSeparator>(
//            IParser<TInput, TMatch> item, IParser<TInput, TSeparator> separator)
//        {
//            Preconditions.NotNull(item, nameof(item));
//            Preconditions.NotNull(separator, nameof(separator));
//
//            return ParserExtensions.Parser<TInput, ImmutableList<TMatch>>(async (input, continuation) =>
//                    {
//                        (await input.Resolve(item)).Apply(
//                            );
//                        (await )
//                        WorkList<TInput> MakeMatchWork(ImmutableList<TMatch> results)
//                        {
//                            WorkList<TInput> Resolve(Resolution<TInput, TMatch> resolution)
//                            {
//                                switch (resolution)
//                                {
//                                    case Match<TInput, TMatch> match:
//                                        var newResults = results.Add(match.Value);
//                                        return WorkListExtensions.Add(
//                                            resolve(new Match<TInput, ImmutableList<TMatch>>(newResults)),
//                                            MakeSeparatorWork(newResults));
//                                    case Failure<TInput, TMatch> failure:
//                                        return resolve(failure.As<ImmutableList<TMatch>>());
//                                    default:
//                                        throw new Exception(); // TODO
//                                }
//                            }
//
//                            return WorkListExtensions.Work(item, Resolve);
//                        }
//
//                        WorkList<TInput> MakeSeparatorWork(ImmutableList<TMatch> results)
//                        {
//                            WorkList<TInput> Resolve(Resolution<TInput, TSeparator> match) => MakeMatchWork(results);
//
//                            return WorkListExtensions.Work(separator, Resolve);
//                        }
//
//                        return MakeMatchWork(ImmutableList<TMatch>.Empty);
//                    }
//                )
//                .WithDescription($"({item})+");
//        }
//
//        public static BasicParser<TInput, TMatch> ErrorUntil<TInput, TMatch>(TInput sentinel,
//            Func<TInput, TMatch> matchSelector)
//        {
////            var terminator = Parsers.Value(sentinel);
//            return ParserExtensions.Parser<TInput, TMatch>(resolve =>
//            {
//                WorkList<TInput> MakeWork(ImmutableList<TInput> erroneousInput)
//                {
//                    WorkList<TInput> F2(Resolution<TInput, TMatch> resolution)
//                    {
//                        switch (resolution)
//                        {
//                            case Match<TInput, TMatch> match:
//                                return WorkListExtensions.Add(
//                                    resolve(new Match<TInput, TMatch>(match.Value,
//                                        ImmutableList.Create<object>(erroneousInput))));
//                            case Failure<TInput, TMatch> failure:
//                                return WorkListExtensions.Add(MakeWork(erroneousInput.Add(failure.Element.Value)));
//                            default:
//                                throw new Exception(); // TODO
//                        }
//                    }
//
//                    return WorkListExtensions.Work(terminator, F2);
//                }
//
//                return MakeWork(ImmutableList<TInput>.Empty);
//            });
//        }
//
//        private static Task<Resolution<TInput, TMatch>> ReturnMatch<TMatch>(TMatch value, Input<TInput> next) =>
//            Task.FromResult((Resolution<TInput, TMatch>) new Match<TInput, TMatch>(value, null));
//
//        private static Task<Resolution<TInput, TMatch>>
//            ReturnFailure<TMatch>(string expectation, Input<TInput> input) =>
//            Task.FromResult((Resolution<TInput, TMatch>) new Failure<TInput, TMatch>(expectation, input));
    }
}