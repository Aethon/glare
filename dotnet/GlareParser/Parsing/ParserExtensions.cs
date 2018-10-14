using System;
using static Aethon.Glare.Parsing.WorkListExtensions;
using static Aethon.Glare.Util.Preconditions;

namespace Aethon.Glare.Parsing
{
    /// <summary>
    /// Extension methods and static constructors for parsers.
    /// </summary>
    public static partial class ParserExtensions
    {
        /// <summary>
        /// Creates a new <see cref="T:BasicParser`2"/>.
        /// </summary>
        /// <param name="start">Function to create the work list to start the parser</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TMatch">Parse result type</typeparam>
        /// <returns>The new parser</returns>
        public static BasicParser<TInput, TMatch> Parser<TInput, TMatch>(
            Func<Resolver<TInput, TMatch>, WorkList<TInput>> start) =>
            new BasicParser<TInput, TMatch>("{parser}", start);

        /// <summary>
        /// Creates a new <see cref="T:BasicParser`2"/> with a description.
        /// </summary>
        /// <param name="description">Description of the parser</param>
        /// <param name="start">Function to create the work list to start the parser</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TMatch">Parse result type</typeparam>
        /// <returns>The new parser</returns>
        public static BasicParser<TInput, TMatch> Parser<TInput, TMatch>(string description,
            Func<Resolver<TInput, TMatch>, WorkList<TInput>> start) =>
            new BasicParser<TInput, TMatch>(description, start);

        /// <summary>
        /// Creates a new <see cref="T:DeferredParser`2"/>.
        /// </summary>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TMatch">Parse result type</typeparam>
        /// <returns>The deferred parser</returns>
        public static DeferredParser<TInput, TMatch> Deferred<TInput, TMatch>() => new DeferredParser<TInput, TMatch>();

        /// <summary>
        /// Creates a new parser that transforms the output of this parser
        /// </summary>
        /// <param name="this">Parser to transform</param>
        /// <param name="transform">Transform function</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TMatch">This parser's match type</typeparam>
        /// <typeparam name="TResult">Transformed type</typeparam>
        /// <returns></returns>
        public static BasicParser<TInput, TResult> As<TInput, TMatch, TResult>(this IParser<TInput, TMatch> @this,
            Func<TMatch, TResult> transform) =>
            Parser<TInput, TResult>(@this.ToString(), resolver => Work(@this, match => resolver(transform(match))));

        /// <summary>
        /// Creates a new parser that executes this parser and then a second parser
        /// </summary>
        /// <param name="this">Parser to execute first</param>
        /// <param name="next">Factory to create the second parser to execute</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TMatch1">This parser's match type</typeparam>
        /// <typeparam name="TMatch2">Second parser's match type</typeparam>
        /// <returns></returns>
        public static BasicParser<TInput, TMatch2> Then<TInput, TMatch1, TMatch2>(this IParser<TInput, TMatch1> @this,
            Func<TMatch1, IParser<TInput, TMatch2>> next) =>
            Parser<TInput, TMatch2>(resolver => Work(@this, match1 => Work(next(match1), resolver)));

//         public static IParser<TInput, TResult2> SelectMany<TInput, TResult1, TResult2>(
//            this IParser<TInput, TResult1> @this,
//            Func<TResult1, IParser<TInput, TResult2>> next) => Then(@this, next);

        /// <summary>
        /// Binds the output of this parser to another parser and transforms the results of both parsers to
        /// and final values.
        /// This method allows C# Linq comprehension syntax to work with the parsers.
        /// </summary>
        /// <param name="this">First parser to execute</param>
        /// <param name="next">Factory to create the second parser to execute</param>
        /// <param name="resultSelector">Transform function</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TMatch1">This parser's match type</typeparam>
        /// <typeparam name="TMatch2">Second parser's match type</typeparam>
        /// <typeparam name="TResult">Transformed type</typeparam>
        /// <returns></returns>
        public static IParser<TInput, TResult> SelectMany<TInput, TMatch1, TMatch2, TResult>(
            this IParser<TInput, TMatch1> @this, Func<TMatch1, IParser<TInput, TMatch2>> next,
            Func<TMatch1, TMatch2, TResult> resultSelector)
        {
            NotNull(@this, nameof(@this));
            NotNull(next, nameof(next));
            NotNull(resultSelector, nameof(resultSelector));
            return Parser<TInput, TResult>(resolver => Work(@this, match1 => Work(next(match1), match2 => resolver(resultSelector(match1,  match2)))));
        }
    }
}