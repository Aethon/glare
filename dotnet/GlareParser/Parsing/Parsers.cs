using System;
using System.Net;
using System.Threading.Tasks;
using static Aethon.Glare.Parsing.ParserExtensions;
using static Aethon.Glare.Util.Preconditions;
using static Aethon.Glare.Parsing.ParseResults;

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
        public BasicParser<TInput, TValue> Return<TValue>(TValue value)
        {
            NotNull(value, nameof(value));
            return Parser<TInput, TValue>(input => Task.FromResult(ParseMatch(value, input)))
                .WithDescription($"{{Return({value})}}");
        }
//
//        /// <summary>
//        /// Creates a parser that matches a single input element based on a predicate.
//        /// </summary>
//        /// <param name="predicate">Predicate to determine if the input element is a match</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <returns>The new parser</returns>
//        public BasicParser<TInput, TInput> Match(Predicate<TInput> predicate)
//        {
//            NotNull(predicate, nameof(predicate));
//            return Parser<TInput, TInput>(input =>
//                    input.Select(
//                        element => predicate(element.Value)
//                            ? ParseMatch(element.Value, element.Next)
//                            : ParseNothing<TInput, TInput>($"a match", element.Position),
//                        end => ParseNothing<TInput, TInput>("a match", end.Position)
//                    )
//                )
//                .WithDescription($"{{Predicate<{typeof(TInput).Name}>}}");
//        }
//
//        /// <summary>
//        /// Creates a parser that matches a single input element exactly.
//        /// </summary>
//        /// <param name="value">Value to match</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <returns>The new parser</returns>
//        public BasicParser<TInput, TInput> Value(TInput value)
//        {
//            NotNull(value, nameof(value));
//            return Match(i => value.Equals(i))
//                .WithDescription(value.ToString());
//        }

        /// <summary>
        /// Creates a new <see cref="T:DeferredParser`2"/>.
        /// </summary>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TMatch">Parse result type</typeparam>
        /// <returns>The deferred parser</returns>
//        public DeferredParser<TInput, TMatch> Deferred<TMatch>() => new DeferredParser<TInput, TMatch>();
    }
}