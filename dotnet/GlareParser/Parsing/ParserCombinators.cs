using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Xml.XPath;
using Aethon.Glare.Util;

namespace Aethon.Glare.Parsing
{
    /// <summary>
    /// Factories for creating general Glare parsers.
    /// </summary>
    public static class ParserCombinators
    {
        /// <summary>
        /// Creates a parser that applies another parser and also matches nothing.
        /// </summary>
        /// <param name="parser">Parser to match</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TMatch">Parser match type</typeparam>
        /// <returns>The new parser</returns>
        public static BasicParser<TInput, Maybe<TMatch>> Optional<TInput, TMatch>(IParser<TInput, TMatch> parser) =>
            ParserExtensions.Parser<TInput, Maybe<TMatch>>(resolve => WorkListExtensions.Add(resolve(new Match<TInput, Maybe<TMatch>>(Maybe<TMatch>.Empty)), parser, resolution =>
                {
                    switch (resolution)
                    {
                        case Match<TInput, TMatch> match:
                            return resolve(new Match<TInput, Maybe<TMatch>>(new Maybe<TMatch>(match.Value)));
                        case Failure<TInput, TMatch> failure:
                            return resolve(new Match<TInput, Maybe<TMatch>>(Maybe<TMatch>.Empty));
                        default:
                            throw new Exception(); // TODO
                    }
                }))
                .WithDescription($"({parser})?");

        /// <summary>
        /// Creates a parser that starts many parsers in parallel.
        /// </summary>
        /// <param name="options">Parsers to start (in parallel)</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TMatch">Parser match type</typeparam>
        /// <returns>The new parser</returns>
        public static BasicParser<TInput, TMatch> OneOf<TInput, TMatch>(params IParser<TInput, TMatch>[] options)
        {
            Preconditions.NotNullOrEmpty(options, nameof(options));
            return ParserExtensions.Parser<TInput,TMatch>(
                    resolve =>
                    {
                        return options.Aggregate(WorkList<TInput>.Nothing, (acc, option) => WorkListExtensions.Add<TInput, TMatch>(acc, option, resolve));
                    }
                )
                .WithDescription($"({string.Join<IParser<TInput, TMatch>>(" | ", options)})");
        }

        /// <summary>
        /// Creates a parser that starts a parser repeatedly, zero or more times.
        /// </summary>
        /// <param name="item">Parser to start</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TMatch">Parser match type</typeparam>
        /// <returns>The new parser</returns>
        public static BasicParser<TInput, ImmutableList<TMatch>> ZeroOrMore<TInput, TMatch>(IParser<TInput, TMatch> item)
        {
            Preconditions.NotNull(item, nameof(item));
            return ParserExtensions.Parser<TInput, ImmutableList<TMatch>>(
                    resolve => WorkListExtensions.Add(resolve(new Match<TInput, ImmutableList<TMatch>>(ImmutableList<TMatch>.Empty)), OneOrMore(item), resolve)
                )
                .WithDescription($"({item})*");
        }

        /// <summary>
        /// Creates a parser that starts a parser repeatedly, one or more times.
        /// </summary>
        /// <param name="item">Parser to start</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TMatch">Parser match type</typeparam>
        /// <returns>The new parser</returns>
        public static BasicParser<TInput, ImmutableList<TMatch>> OneOrMore<TInput, TMatch>(IParser<TInput, TMatch> item)
        {
            Preconditions.NotNull(item, nameof(item));
            return ParserExtensions.Parser<TInput, ImmutableList<TMatch>>(resolve =>
                    {
                        WorkList<TInput> MakeWork(ImmutableList<TMatch> results)
                        {
                            WorkList<TInput> F2(Resolution<TInput, TMatch> resolution)
                            {
                                switch (resolution)
                                {
                                    case Match<TInput, TMatch> match:
                                        var newResults = results.Add(match.Value);
                                        return WorkListExtensions.Add(
                                            resolve(new Match<TInput, ImmutableList<TMatch>>(newResults)),
                                            MakeWork(newResults));
                                    case Failure<TInput, TMatch> failure:
                                        return WorkListExtensions.Add(
                                            resolve(failure.As<ImmutableList<TMatch>>()));
                                    default:
                                        throw new Exception(); // TODO
                                }
                            }

                            return WorkListExtensions.Work(item, F2);
                        }

                        return MakeWork(ImmutableList<TMatch>.Empty);
                    }
                )
                .WithDescription($"({item})+");
        }

        /// <summary>
        /// Creates a parser that matches one or more items interspersed with a separator.
        /// </summary>
        /// <param name="item">Parser to match items</param>
        /// <param name="separator">Parser to match separators</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TMatch">Parser match type</typeparam>
        /// <typeparam name="TSeparator">Separator parser match type</typeparam>
        /// <returns>The new parser</returns>
        public static BasicParser<TInput, ImmutableList<TMatch>> SeparatedList<TInput, TMatch, TSeparator>(
            IParser<TInput, TMatch> item, IParser<TInput, TSeparator> separator)
        {
            var listParser = NonEmptySeparatedList(item, separator);
            return ParserExtensions.Parser<TInput, ImmutableList<TMatch>>(resolve =>
                WorkListExtensions.Add(resolve(new Match<TInput, ImmutableList<TMatch>>(ImmutableList<TMatch>.Empty)), listParser, resolve));
        }

        /// <summary>
        /// Creates a parser that any number of items interspersed with a separator.
        /// </summary>
        /// <param name="item">Parser to match items</param>
        /// <param name="separator">Parser to match separators</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TMatch">Parser match type</typeparam>
        /// <typeparam name="TSeparator">Separator parser match type</typeparam>
        /// <returns>The new parser</returns>
        public static BasicParser<TInput, ImmutableList<TMatch>> NonEmptySeparatedList<TInput, TMatch, TSeparator>(
            IParser<TInput, TMatch> item, IParser<TInput, TSeparator> separator)
        {
            Preconditions.NotNull(item, nameof(item));
            Preconditions.NotNull(separator, nameof(separator));
            
            return ParserExtensions.Parser<TInput, ImmutableList<TMatch>>(resolve =>
                    {
                        WorkList<TInput> MakeMatchWork(ImmutableList<TMatch> results)
                        {
                            WorkList<TInput> Resolve(Resolution<TInput, TMatch> resolution)
                            {
                                switch (resolution)
                                {
                                    case Match<TInput, TMatch> match:
                                        var newResults = results.Add(match.Value);
                                        return WorkListExtensions.Add(resolve(new Match<TInput, ImmutableList<TMatch>>(newResults)),
                                            MakeSeparatorWork(newResults));
                                    case Failure<TInput, TMatch> failure:
                                        return resolve(failure.As<ImmutableList<TMatch>>());
                                    default:
                                        throw new Exception(); // TODO
                                }
                            }

                            return WorkListExtensions.Work(item, Resolve);
                        }

                        WorkList<TInput> MakeSeparatorWork(ImmutableList<TMatch> results)
                        {
                            WorkList<TInput> Resolve(Resolution<TInput, TSeparator> match) => MakeMatchWork(results);
                            
                            return WorkListExtensions.Work(separator, Resolve);
                        }
                        return MakeMatchWork(ImmutableList<TMatch>.Empty);
                    }
                )
                .WithDescription($"({item})+");
        }
        
        public static BasicParser<TInput, TMatch> ErrorUntil<TInput, TMatch>(TInput sentinel, Func<TInput, TMatch> matchSelector)
        {
            var terminator = Parsers.Value(sentinel);
            return ParserExtensions.Parser<TInput, TMatch>(resolve =>
            {
                WorkList<TInput> MakeWork(ImmutableList<TInput> erroneousInput)
                {
                    WorkList<TInput> F2(Resolution<TInput, TMatch> resolution)
                    {
                        switch (resolution)
                        {
                            case Match<TInput, TMatch> match:
                                return WorkListExtensions.Add(
                                    resolve(new Match<TInput, TMatch>(match.Value,
                                        ImmutableList.Create<object>(erroneousInput))));
                            case Failure<TInput, TMatch> failure:
                                return WorkListExtensions.Add(MakeWork(erroneousInput.Add(failure.Element.Value)));
                            default:
                                throw new Exception(); // TODO
                        }
                    }

                    return WorkListExtensions.Work(terminator, F2);
                }

                return MakeWork(ImmutableList<TInput>.Empty);
            });
        }
    }
}