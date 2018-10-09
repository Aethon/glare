using System;
using System.Collections.Immutable;
using System.Linq;
using Aethon.Glare.Parsing.ParseTree;
using static Aethon.Glare.Parsing.WorkListExtensions;
using static Aethon.Glare.Parsing.ParserExtensions;
using static Aethon.Glare.Util.Preconditions;
using static Aethon.Glare.Parsing.ParseStateExtensions;

namespace Aethon.Glare.Parsing
{
    public static class Parsers
    {
        /// <summary>
        /// Creates a parser that matches a single input element based on a predicate.
        /// </summary>
        /// <param name="predicate">Predicate to determine if the input element is a match</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The new parser</returns>
        public static BasicParser<T> Match<T>(Predicate<T> predicate)
        {
            NotNull(predicate, nameof(predicate));
            return Parser<T>(resolve => Work<T>(
                    input => predicate(input)
                        ? resolve(ParsedValue(input))
                        : ParseState<T>.Nothing)
                )
                .WithDescription($"{{Predicate<{typeof(T).Name}>}}");
        }

        /// <summary>
        /// Creates a parser that matches a single input element exactly.
        /// </summary>
        /// <param name="value">Value to match</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The new parser</returns>
        public static BasicParser<T> Value<T>(T value)
        {
            NotNull(value, nameof(value));
            return Match<T>(i => value.Equals(i))
                .WithDescription(value.ToString());
        }

        /// <summary>
        /// Creates a parser that applies another parser and also matches nothing.
        /// </summary>
        /// <param name="parser">Parser to match</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The new parser</returns>
        public static BasicParser<T> Optional<T>(IParser<T> parser) =>
            Parser<T>(resolve => Work<T>(i => resolve(new MissingValue())).Add(parser, resolve))
                .WithDescription($"({parser})?");

        /// <summary>
        /// Creates a parser that starts a sequence of parsers in order.
        /// </summary>
        /// <param name="items">Parsers to start (in order)</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The new parser</returns>
        public static BasicParser<T> Sequence<T>(params IParser<T>[] items)
        {
            NotNullOrEmpty(items, nameof(items));
            return Parser<T>(resolve =>
                {
                    WorkList<T> MakeWork(ImmutableList<ParseNode> results)
                    {
                        ParseState<T> F2(ParseNode match)
                        {
                            var newResults = results.Add(match);
                            return newResults.Count == items.Length
                                ? resolve(new ParsedSequence(newResults))
                                : State(MakeWork(newResults));
                        }

                        return Work(items[results.Count], F2);
                    }

                    return MakeWork(ImmutableList<ParseNode>.Empty);
                }
            ).WithDescription($"[{string.Join<IParser<T>>(", ", items)}]");
        }

        /// <summary>
        /// Creates a parser that starts many parsers in parallel.
        /// </summary>
        /// <param name="options">Parsers to start (in parallel)</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The new parser</returns>
        public static BasicParser<T> OneOf<T>(params IParser<T>[] options)
        {
            NotNullOrEmpty(options, nameof(options));
            return Parser<T>(
                    resolve =>
                    {
                        return options.Aggregate(WorkList<T>.Nothing, (acc, option) => acc.Add(option, resolve));
                    }
                )
                .WithDescription($"({string.Join<IParser<T>>(" | ", options)})");
        }

        /// <summary>
        /// Creates a parser that starts a parser repeatedly, zero or more times.
        /// </summary>
        /// <param name="item">Parser to start</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The new parser</returns>
        public static BasicParser<T> ZeroOrMore<T>(IParser<T> item)
        {
            NotNull(item, nameof(item));
            return Parser<T>(
                    resolve => Work(OneOrMore(item), resolve)
                        .Add(input => resolve(new ParsedSequence(ImmutableList<ParseNode>.Empty)))
                )
                .WithDescription($"({item})*");
        }

        /// <summary>
        /// Creates a parser that starts a parser repeatedly, one or more times.
        /// </summary>
        /// <param name="item">Parser to start</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The new parser</returns>
        public static BasicParser<T> OneOrMore<T>(IParser<T> item)
        {
            NotNull(item, nameof(item));
            return Parser<T>(resolve =>
                    {
                        WorkList<T> MakeWork(ImmutableList<ParseNode> results)
                        {
                            ParseState<T> F2(ParseNode match)
                            {
                                var newResults = results.Add(match);
                                return resolve(new ParsedSequence(newResults))
                                    .Add(MakeWork(newResults));
                            }

                            return Work(item, F2);
                        }

                        return MakeWork(ImmutableList<ParseNode>.Empty);
                    }
                )
                .WithDescription($"({item})+");
        }
        
        private static ParseNode ParsedValue<T>(T value) => new ParsedValue<T>(value);
    }
}