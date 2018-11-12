using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
        /// <typeparam name="E">Input element type</typeparam>
        /// <typeparam name="M">Parse result type</typeparam>
        /// <returns>The new parser</returns>
        public static BasicParser<E, M> Parser<E, M>(
            ParseMethod<E, M> resolver)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            return new BasicParser<E, M>("{parser}", resolver);
        }

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
//        /// Creates a new parser that executes this parser and then a second parser
//        /// </summary>
//        /// <param name="this">Parser to execute first</param>
//        /// <param name="next">Factory to create the second parser to execute</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TMatch1">This parser's match type</typeparam>
//        /// <typeparam name="TMatch2">Second parser's match type</typeparam>
//        /// <returns></returns>
        public static BasicParser<E, M2> Bind<E, M1, M2>(this IParser<E, M1> @this,
            Func<M1, IParser<E, M2>> next) =>
            Parser<E, M2>(async input =>
            {
                return await (await input.Resolve(@this)).Bind(next);
//                {
//                    case Match<TInput, TMatch1> match:
//                        return (await Task.WhenAll(match.Alternatives.Select(alt =>
//                                alt.RemainingInput.Resolve(next(alt.Value)))))
//                            .Aggregate((a, r) => a.And(r));
//                    case Nothing<TInput, TMatch1> nothing:
//                        return nothing.As<TMatch2>();
//                    default:
//                        throw new Exception();
//                }
            });

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
            return Parser<E, T>(async input =>
            {
                return await (await input.Resolve(@this)).Bind(async (m1, r1) =>
                    (await next(m1).Bind(m2 => Parsers<E>.Return(resultSelector(m1, m2))).Resolve(r1)));
            });
        }
    }
}