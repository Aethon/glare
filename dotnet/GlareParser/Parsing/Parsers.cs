using System;
using System.Threading.Tasks;
using static Aethon.Glare.Parsing.ParserExtensions;
using static Aethon.Glare.Util.Preconditions;
using static Aethon.Glare.Parsing.ParseStuff;

namespace Aethon.Glare.Parsing
{
//    /// <summary>
//    /// Factories for creating general Glare parsers.
//    /// </summary>
    public static class Parsers<E>
    {
//        /// <summary>
//        /// Creates a parser that resolves a match without consuming the input stream.
//        /// </summary>
//        /// <param name="value">Value the parser will resolve</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TValue">Value type (parser match type)</typeparam>
//        /// <returns>The new parser</returns>
        public static BasicParser<E, M> Return<M>(M value)
        {
            NotNull(value, nameof(value));
            return Parser<E, M>(input => Task.FromResult(SingleMatch(value, input)))
                .WithDescription($"{{Return({value})}}");
        }

        //        /// <summary>
//        /// Creates a parser that resolves a match without consuming the input stream.
//        /// </summary>
//        /// <param name="value">Value the parser will resolve</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TValue">Value type (parser match type)</typeparam>
//        /// <returns>The new parser</returns>
        public static BasicParser<E, NoValue> MatchEnd()
        {
            return Parser<E, NoValue>(input =>
                    Task.FromResult(
                        input.Select(
                            element => Nothing<E, NoValue>("end", element.Position),
                            end => SingleMatch(NoValue.Instance, end)
                        )
                    )
                )
                .WithDescription($"{{End}}");
        }
//
//        /// <summary>
//        /// Creates a parser that matches a single input element based on a predicate.
//        /// </summary>
//        /// <param name="predicate">Predicate to determine if the input element is a match</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <returns>The new parser</returns>
        public static BasicParser<E, E> Match(Predicate<E> predicate)
        {
            NotNull(predicate, nameof(predicate));
            return Parser<E, E>(input =>
                    Task.FromResult(
                        input.Select(
                            element => predicate(element.Value)
                                ? SingleMatch(element.Value, element.Next())
                                : Nothing<E, E>("a match", 0), // TODO
                            end => Nothing<E, E>("a match", 0) // TODO
                        )
                    )
                )
                .WithDescription($"{{Predicate<{typeof(E).Name}>}}");
        }

//        /// <summary>
//        /// Creates a parser that matches a single input element exactly.
//        /// </summary>
//        /// <param name="value">Value to match</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <returns>The new parser</returns>
        public static BasicParser<E, E> Value(E value)
        {
            NotNull(value, nameof(value));
            return Match(i => value.Equals(i))
                .WithDescription(value.ToString());
        }

        /// <summary>
        /// Creates a new <see cref="T:DeferredParser`2"/>.
        /// </summary>
        /// <typeparam name="E">Input element type</typeparam>
        /// <typeparam name="M">Parse result type</typeparam>
        /// <returns>The deferred parser</returns>
        public static DeferredParser<E, M> Deferred<M>() => new DeferredParser<E, M>();
    }
}