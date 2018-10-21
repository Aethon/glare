//using System;
//using System.Collections.Immutable;
//using System.Linq;
//using System.Net;
//using Aethon.Glare.Scanning;
//using static Aethon.Glare.Parsing.WorkListExtensions;
//using static Aethon.Glare.Parsing.ParserExtensions;
//using static Aethon.Glare.Util.Preconditions;
//
//namespace Aethon.Glare.Parsing
//{
//    public static class WordMarkSpaceParsers
//    {
////        /// <summary>
////        /// Creates a parser that resolves a match without consuming the input stream.
////        /// </summary>
////        /// <param name="value">Value the parser will resolve</param>
////        /// <typeparam name="TInput">Input element type</typeparam>
////        /// <typeparam name="TValue">Value type (parser match type)</typeparam>
////        /// <returns>The new parser</returns>
////        public static BasicParser<TInput, TValue> Value<TInput, TValue>(TValue value)
////        {
////            NotNull(value, nameof(value));
////            return Parser<TInput, TValue>(resolve => resolve(value))
////                .WithDescription($"{{Predicate<{typeof(TInput).Name}>}}");
////        }
//        
//        /// <summary>
//        /// Creates a parser that matches a single input element based on a predicate.
//        /// </summary>
//        /// <param name="predicate">Predicate to determine if the input element is a match</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <returns>The new parser</returns>
//        public static BasicParser<TInput, TInput> Match<TInput>(Predicate<TInput> predicate)
//        {
//            NotNull(predicate, nameof(predicate));
//            return Parser<TInput, TInput>(resolve => Work<TInput>(
//                    input => predicate(input)
//                        ? resolve(input)
//                        : WorkList<TInput>.Nothing
//                ))
//                .WithDescription($"{{Predicate<{typeof(TInput).Name}>}}");
//        }
//
//        /// <summary>
//        /// Creates a parser that matches a single input element exactly.
//        /// </summary>
//        /// <param name="value">Value to match</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <returns>The new parser</returns>
//        public static BasicParser<ScanToken, ScanToken> Word(string value)
//        {
//            NotNull(value, nameof(value));
//            return Match<ScanToken>(input => input.Type == ScanTokenType.Word && input.Text == value)
//                .WithDescription($"\"{value}\"");
//        }
//
////        /// <summary>
////        /// Creates a parser that matches a single input element exactly.
////        /// </summary>
////        /// <param name="value">Value to match</param>
////        /// <typeparam name="TInput">Input element type</typeparam>
////        /// <returns>The new parser</returns>
////        public static BasicParser<ScanToken, ScanToken> WordMatching(string regex)
////        {
////            NotNull(regex, nameof(regex));
////            return Match<ScanToken>(input => input.Type == ScanTokenType.Word && input.Text == value)
////                .WithDescription($"\"{value}\"");
////        }
//        
//        /// <summary>
//        /// Creates a parser that starts many parsers in parallel.
//        /// </summary>
//        /// <param name="options">Parsers to start (in parallel)</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TMatch">Parser match type</typeparam>
//        /// <returns>The new parser</returns>
//        public static BasicParser<TInput, TMatch> OneOf<TInput, TMatch>(params IParser<TInput, TMatch>[] options)
//        {
//            NotNullOrEmpty(options, nameof(options));
//            return Parser<TInput,TMatch>(
//                    resolve =>
//                    {
//                        return options.Aggregate(WorkList<TInput>.Nothing, (acc, option) => acc.Add(option, resolve));
//                    }
//                )
//                .WithDescription($"({string.Join<IParser<TInput, TMatch>>(" | ", options)})");
//        }
//
//        /// <summary>
//        /// Creates a parser that starts a parser repeatedly, zero or more times.
//        /// </summary>
//        /// <param name="item">Parser to start</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TMatch">Parser match type</typeparam>
//        /// <returns>The new parser</returns>
//        public static BasicParser<TInput, ImmutableList<TMatch>> ZeroOrMore<TInput, TMatch>(IParser<TInput, TMatch> item)
//        {
//            NotNull(item, nameof(item));
//            return Parser<TInput, ImmutableList<TMatch>>(
//                    resolve => resolve(ImmutableList<TMatch>.Empty).Add(OneOrMore(item), resolve)
//                )
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
//        public static BasicParser<TInput, ImmutableList<TMatch>> OneOrMore<TInput, TMatch>(IParser<TInput, TMatch> item)
//        {
//            NotNull(item, nameof(item));
//            return Parser<TInput, ImmutableList<TMatch>>(resolve =>
//                    {
//                        WorkList<TInput> MakeWork(ImmutableList<TMatch> results)
//                        {
//                            WorkList<TInput> F2(TMatch match)
//                            {
//                                var newResults = results.Add(match);
//                                return resolve(newResults)
//                                    .Add(MakeWork(newResults));
//                            }
//
//                            return Work(item, F2);
//                        }
//
//                        return MakeWork(ImmutableList<TMatch>.Empty);
//                    }
//                )
//                .WithDescription($"({item})+");
//        }
//
//        public static BasicParser<TInput, ImmutableList<TMatch>> SeparatedList<TInput, TMatch, TSeparator>(
//            IParser<TInput, TMatch> item, IParser<TInput, TSeparator> separator)
//        {
//            var listParser = NonEmptySeparatedList(item, separator);
//            return Parser<TInput, ImmutableList<TMatch>>(resolve =>
//                resolve(ImmutableList<TMatch>.Empty).Add(listParser, resolve));
//        }
//
//        public static BasicParser<TInput, ImmutableList<TMatch>> NonEmptySeparatedList<TInput, TMatch, TSeparator>(
//            IParser<TInput, TMatch> item, IParser<TInput, TSeparator> separator)
//        {
//            NotNull(item, nameof(item));
//            NotNull(separator, nameof(separator));
//            
//            return Parser<TInput, ImmutableList<TMatch>>(resolve =>
//                    {
//                        WorkList<TInput> MakeMatchWork(ImmutableList<TMatch> results)
//                        {
//                            WorkList<TInput> Resolve(TMatch match)
//                            {
//                                var newResults = results.Add(match);
//                                return resolve(newResults)
//                                    .Add(MakeSeparatorWork(newResults));
//                            }
//
//                            return Work(item, Resolve);
//                        }
//
//                        WorkList<TInput> MakeSeparatorWork(ImmutableList<TMatch> results)
//                        {
//                            WorkList<TInput> Resolve(TSeparator match) => MakeMatchWork(results);
//                            
//                            return Work(separator, Resolve);
//                        }
//                        return MakeMatchWork(ImmutableList<TMatch>.Empty);
//                    }
//                )
//                .WithDescription($"({item})+");
//        }
//        
//        /// <summary>
//        /// Creates a new <see cref="T:DeferredParser`2"/>.
//        /// </summary>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TMatch">Parse result type</typeparam>
//        /// <returns>The deferred parser</returns>
//        public static DeferredParser<TInput, TMatch> Deferred<TInput, TMatch>() => new DeferredParser<TInput, TMatch>();
//
//    }
//}