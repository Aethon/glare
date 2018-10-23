using System;
using System.Net;
using static Aethon.Glare.Parsing.WorkListExtensions;
using static Aethon.Glare.Parsing.ParserExtensions;
using static Aethon.Glare.Util.Preconditions;

namespace Aethon.Glare.Parsing
{
    /// <summary>
    /// Factories for creating general Glare parsers.
    /// </summary>
    public class Parsers<TInput>
    {
        /// <summary>
        /// Creates a parser that resolves a match without consuming the input stream.
        /// </summary>
        /// <param name="value">Value the parser will resolve</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TValue">Value type (parser match type)</typeparam>
        /// <returns>The new parser</returns>
        public BasicParser<TInput, TValue> Value<TValue>(TValue value)
        {
            NotNull(value, nameof(value));
            return Parser<TInput, TValue>(resolve => resolve(value))
                .WithDescription($"{{Predicate<{typeof(TInput).Name}>}}");
        }

        /// <summary>
        /// Creates a parser that matches a single input element based on a predicate.
        /// </summary>
        /// <param name="predicate">Predicate to determine if the input element is a match</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <returns>The new parser</returns>
        public BasicParser<TInput, TInput> Match(Predicate<TInput> predicate)
        {
            NotNull(predicate, nameof(predicate));
            return Parser<TInput, TInput>(resolve => Work<TInput>(
                    input => predicate(input)
                        ? resolve(input)
                        : WorkList<TInput>.Nothing
                ))
                .WithDescription($"{{Predicate<{typeof(TInput).Name}>}}");
        }

        /// <summary>
        /// Creates a parser that matches a single input element exactly.
        /// </summary>
        /// <param name="value">Value to match</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <returns>The new parser</returns>
        public BasicParser<TInput, TInput> Input(TInput value)
        {
            NotNull(value, nameof(value));
            return Match(i => value.Equals(i))
                .WithDescription(value.ToString());
        }
                 
        /// <summary>
        /// Creates a new <see cref="T:DeferredParser`2"/>.
        /// </summary>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TMatch">Parse result type</typeparam>
        /// <returns>The deferred parser</returns>
        public DeferredParser<TInput, TMatch> Deferred<TMatch>() => new DeferredParser<TInput, TMatch>();
    }
}