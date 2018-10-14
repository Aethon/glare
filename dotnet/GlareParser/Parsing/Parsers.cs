using System;
using System.Collections.Immutable;
using System.Linq;
using static Aethon.Glare.Parsing.WorkListExtensions;
using static Aethon.Glare.Parsing.ParserExtensions;
using static Aethon.Glare.Util.Preconditions;

namespace Aethon.Glare.Parsing
{
    public static class Parsers
    {
        /// <summary>
        /// Creates a parser that matches a single input element based on a predicate.
        /// </summary>
        /// <param name="predicate">Predicate to determine if the input element is a match</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <returns>The new parser</returns>
        public static BasicParser<TInput, TInput> Match<TInput>(Predicate<TInput> predicate)
        {
            NotNull(predicate, nameof(predicate));
            return Parser<TInput, TInput>(resolve => Work<TInput>(
                    input => predicate(input)
                        ? resolve(input)
                        : WorkList<TInput>.Nothing)
                )
                .WithDescription($"{{Predicate<{typeof(TInput).Name}>}}");
        }

        /// <summary>
        /// Creates a parser that matches a single input element exactly.
        /// </summary>
        /// <param name="value">Value to match</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <returns>The new parser</returns>
        public static BasicParser<TInput, TInput> Value<TInput>(TInput value)
        {
            NotNull(value, nameof(value));
            return Match<TInput>(i => value.Equals(i))
                .WithDescription(value.ToString());
        }

        /// <summary>
        /// Creates a parser that applies another parser and also matches nothing.
        /// </summary>
        /// <param name="parser">Parser to match</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TMatch">Parser match type</typeparam>
        /// <returns>The new parser</returns>
        public static BasicParser<TInput, Maybe<TMatch>> Optional<TInput, TMatch>(IParser<TInput, TMatch> parser) =>
            Parser<TInput, Maybe<TMatch>>(resolve => Work<TInput>(i => resolve(Maybe<TMatch>.Empty)).Add(parser, t => resolve(new Maybe<TMatch>(t))))
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
            NotNullOrEmpty(options, nameof(options));
            return Parser<TInput,TMatch>(
                    resolve =>
                    {
                        return options.Aggregate(WorkList<TInput>.Nothing, (acc, option) => acc.Add(option, resolve));
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
            NotNull(item, nameof(item));
            return Parser<TInput, ImmutableList<TMatch>>(
                    resolve => Work(OneOrMore(item), resolve)
                        .Add(input => resolve(ImmutableList<TMatch>.Empty))
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
            NotNull(item, nameof(item));
            return Parser<TInput, ImmutableList<TMatch>>(resolve =>
                    {
                        WorkList<TInput> MakeWork(ImmutableList<TMatch> results)
                        {
                            WorkList<TInput> F2(TMatch match)
                            {
                                var newResults = results.Add(match);
                                return resolve(newResults)
                                    .Add(MakeWork(newResults));
                            }

                            return Work(item, F2);
                        }

                        return MakeWork(ImmutableList<TMatch>.Empty);
                    }
                )
                .WithDescription($"({item})+");
        }
    }
}