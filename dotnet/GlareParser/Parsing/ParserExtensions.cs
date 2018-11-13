using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Aethon.Glare.Parsing.Parsers;
using static Aethon.Glare.Util.Preconditions;

namespace Aethon.Glare.Parsing
{
    /// <summary>
    /// Extension methods and static constructors for parsers.
    /// </summary>
    public static class ParserExtensions
    {
        //        /// <summary>
//        /// Creates a new parser that executes this parser and then a second parser
//        /// </summary>
//        /// <param name="this">Parser to execute first</param>
//        /// <param name="next">Factory to create the second parser to execute</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TMatch1">This parser's match type</typeparam>
//        /// <typeparam name="TMatch2">Second parser's match type</typeparam>
//        /// <returns></returns>
        public static IParser<E, M2> Bind<E, M1, M2>(this IParser<E, M1> @this, Func<M1, IParser<E, M2>> nextParserSelector) =>
            new BindingParser<E,M1,M2>(@this, nextParserSelector);

//
//        /// <summary>
//        /// Binds the output of this parser to another parser and transforms the results of both parsers to
//        /// and final values.
//        /// This method allows C# Linq comprehension syntax to work with the parsers.
//        /// </summary>
//        /// <param name="this">First parser to execute</param>
//        /// <param name="next">Factory to create the second parser to execute</param>
//        /// <param name="resultSelector">Transform function</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TMatch1">This parser's match type</typeparam>
//        /// <typeparam name="TMatch2">Second parser's match type</typeparam>
//        /// <typeparam name="TResult">Transformed type</typeparam>
//        /// <returns></returns>
        public static IParser<E, T> SelectMany<E, M1, M2, T>(
            this IParser<E, M1> @this, Func<M1, IParser<E, M2>> next,
            Func<M1, M2, T> resultSelector)
        {
            NotNull(@this, nameof(@this));
            NotNull(next, nameof(next));
            NotNull(resultSelector, nameof(resultSelector));

            return @this.Bind(m1 => next(m1).Bind(m2 => ParserFactory.Return<E, T>(resultSelector(m1, m2))));
//            return ParserFactory.Parser<E, T>(input => input.Resolve(binding));
        }

        public static IParser<E, M2> As<E, M1, M2>(this IParser<E, M1> @this, Func<M1, M2> selector) =>
            new TransformedParser<E, M1, M2>(@this, selector);
    }
}