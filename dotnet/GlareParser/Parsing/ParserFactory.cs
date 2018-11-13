using System;
using System.Collections.Immutable;
using Aethon.Glare.Parsing.Parsers;

namespace Aethon.Glare.Parsing
{
    public static class ParserFactory {
        //        /// <summary>
//        /// Creates a parser that resolves a match without consuming the input stream.
//        /// </summary>
//        /// <param name="value">Value the parser will resolve</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TValue">Value type (parser match type)</typeparam>
//        /// <returns>The new parser</returns>
        public static IParser<E, M> Return<E, M>(M value) => new ReturnParser<E, M>(value);
//        {
//            NotNull(value, nameof(value));
//            return Parser<E, M>(input => Task.FromResult(SingleMatch(value, input)))
//                .WithDescription($"{{Return({value})}}");
//        }

        //        /// <summary>
//        /// Creates a parser that resolves a match without consuming the input stream.
//        /// </summary>
//        /// <param name="value">Value the parser will resolve</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TValue">Value type (parser match type)</typeparam>
//        /// <returns>The new parser</returns>
        public static IParser<E, NoValue> End<E>() => EndParser<E>.Singleton;
//        {
//            return Parser<E, NoValue>(input =>
//                    Task.FromResult(
//                        input.Select(
//                            element => Nothing<E, NoValue>("end", element.Position),
//                            end => SingleMatch(NoValue.Instance, end)
//                        )
//                    )
//                )
//                .WithDescription($"{{End}}");
//        }
//
//        /// <summary>
//        /// Creates a parser that matches a single input element based on a predicate.
//        /// </summary>
//        /// <param name="predicate">Predicate to determine if the input element is a match</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <returns>The new parser</returns>
//        public static BasicParser<E, E> Match(Predicate<E> predicate)
//        {
//            NotNull(predicate, nameof(predicate));
//            return Parser<E, E>(input =>
//                    Task.FromResult(
//                        input.Select(
//                            element => predicate(element.Value)
//                                ? SingleMatch(element.Value, element.Next())
//                                : Nothing<E, E>("a match", 0), // TODO
//                            end => Nothing<E, E>("a match", 0) // TODO
//                        )
//                    )
//                )
//                .WithDescription($"{{Predicate<{typeof(E).Name}>}}");
//        }

//        /// <summary>
//        /// Creates a parser that matches a single input element exactly.
//        /// </summary>
//        /// <param name="value">Value to match</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <returns>The new parser</returns>
        public static IParser<E, E> Value<E>(E value) => new ValueParser<E>(value);

        /// <summary>
        /// Creates a new <see cref="T:DeferredParser`2"/>.
        /// </summary>
        /// <typeparam name="E">Input element type</typeparam>
        /// <typeparam name="M">Parse result type</typeparam>
        /// <returns>The deferred parser</returns>
        public static DeferredParser<E, M> Deferred<E,M>() => new DeferredParser<E, M>();

        /// <summary>
        /// Creates a new <see cref="T:BasicParser`2"/>.
        /// </summary>
        /// <param name="start">Function to create the work list to start the parser</param>
        /// <typeparam name="E">Input element type</typeparam>
        /// <typeparam name="M">Parse result type</typeparam>
        /// <returns>The new parser</returns>
//        public static IParser<E, M> Parser<E, M>(
//            ParseMethod<E, M> resolver)
//        {
//            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
//            return new AnonymousParser<E, M>("{parser}", resolver);
//        }

//        /// <summary>
//        /// Creates a new <see cref="T:BasicParser`2"/> with a description.
//        /// </summary>
//        /// <param name="description">Description of the parser</param>
//        /// <param name="start">Function to create the work list to start the parser</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TMatch">Parse result type</typeparam>
//        /// <returns>The new parser</returns>
//        public static BasicParser<TInput, TMatch> Parser<TInput, TMatch>(string description,
//            Func<Resolver<TInput, TMatch>, WorkList<TInput>> start) =>
//            new BasicParser<TInput, TMatch>(description, start);
//
//        /// <summary>
//        /// Creates a new parser that transforms the output of this parser
//        /// </summary>
//        /// <param name="this">Parser to transform</param>
//        /// <param name="transform">Transform function</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TMatch">This parser's match type</typeparam>
//        /// <typeparam name="TResult">Transformed type</typeparam>
//        /// <returns></returns>
//        public static BasicParser<TInput, TResult> As<TInput, TMatch, TResult>(this IParser<TInput, TMatch> @this,
//            Func<TMatch, TResult> transform) =>
//            Parser<TInput, TResult>(@this.ToString(), resolver => Work(@this, resolution =>
//            {
//                switch (resolution)
//                {
//                    case Match<TInput, TMatch> match:
//                        return resolver(new Match<TInput,TResult>(transform(match.Value)));
//                    case Failure<TInput, TMatch> failure:
//                        return resolver(failure.As<TResult>());
//                    default:
//                        throw new Exception(); // TODO
//                }
//            }));
//


        //        /// <summary>
//        /// Creates a parser that applies another parser and also matches nothing.
//        /// </summary>
//        /// <param name="parser">Parser to match</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TMatch">Parser match type</typeparam>
//        /// <returns>The new parser</returns>
        public static IParser<E, Maybe<M>> Optional<E, M>(IParser<E, M> parser) =>
            OneOf(Return<E,Maybe<M>>(Maybe<M>.Empty), parser.As(m => new Maybe<M>(m)));
//        {
//            var innerParser = parser.Bind(r => Parsers<E>.Return(new Maybe<M>(r)));
//            return Parser<E, Maybe<M>>(async input => SingleMatch(Maybe<M>.Empty, input)
//                    .And(await input.Resolve(innerParser)))
//                .WithDescription($"({parser})?");
//        }

//        /// <summary>
//        /// Creates a parser that starts many parsers in parallel.
//        /// </summary>
//        /// <param name="options">Parsers to start (in parallel)</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TMatch">Parser match type</typeparam>
//        /// <returns>The new parser</returns>
        public static IParser<E, M> OneOf<E, M>(params IParser<E, M>[] options) => new AlternatesParser<E,M>(options);
//        {
//            Preconditions.NotNullOrEmpty(options, nameof(options));
//            return Parser<E, M>(
//                    async input =>
//                        (await Task.WhenAll(options.Select(input.Resolve))).Aggregate((a, m) => a.And(m))
//                )
//                .WithDescription($"({string.Join<IParser<E, M>>(" | ", options)})");
//        }

//
//        /// <summary>
//        /// Creates a parser that starts a parser repeatedly, zero or more times.
//        /// </summary>
//        /// <param name="item">Parser to start</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TMatch">Parser match type</typeparam>
//        /// <returns>The new parser</returns>
        public static IParser<E, ImmutableList<M>> ZeroOrMore<E, M>(
            IParser<E, M> item) => OneOf(Return<E, ImmutableList<M>>(ImmutableList<M>.Empty), OneOrMore(item));
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
        public static IParser<E, ImmutableList<M>> OneOrMore<E, M>(IParser<E, M> item) =>
            new SequenceParser<E, M>(item, ImmutableList<M>.Empty);
//        {
//            Preconditions.NotNull(item, nameof(item));
//            return List(item, ImmutableList<M>.Empty);
//        }

//        private static BasicParser<E, ImmutableList<M>> List<E, M>(IParser<E, M> item,
//            ImmutableList<M> previous)
//        {
//            return Parser<E, ImmutableList<M>>(async input =>
//                    {
//                        var result = await input.Resolve(item);
//                        switch (result)
//                        {
//                            case Match<E, M> match:
//                                var newAlts = match.Alternatives
//                                    .Select(a => Alt(previous.Add(a.Value), a.RemainingInput)).ToImmutableHashSet();
//                                var newTasks = newAlts.Select(a => a.RemainingInput.Resolve(List(item, a.Value)));
//                                var additionalResults = await Task.WhenAll(newTasks);
//                                return additionalResults.Aggregate(Matches(newAlts), (a, m) => a.And(m));
//                            case Nothing<E, M> nothing:
//                                return nothing.As<ImmutableList<M>>();
//                            default:
//                                throw new Exception(); // TODO:
//                        }
//                    }
//                )
//                .WithDescription($"({item})+");
//        }

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