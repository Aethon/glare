//using System;
//using System.Collections.Immutable;
//using System.Net;
//using System.Reflection;
//using Aethon.Glare.Scanning;
//using static Aethon.Glare.Parsing.WorkListExtensions;
//using static Aethon.Glare.Parsing.ParserExtensions;
//using static Aethon.Glare.Util.Preconditions;
//using static Aethon.Glare.Parsing.ParserCombinators;
//
//namespace Aethon.Glare.Parsing
//{
//    /// <summary>
//    /// Factories for creating general Glare character parsers.
//    /// </summary>
//    public abstract class ScanParsers<T>: Parsers<ScanToken>
//    {
//        public BasicParser<ScanToken,Token<T>> LeadingTrivia => ZeroOrMore(OneOf(AnySpace, LineComment, BlockComment, Newline));
//
//        public BasicParser<ScanToken, Token<T>> TrailingTrivia => ZeroOrMore(OneOf(AnySpace.AsList(), LineComment, BlockComment)).Then(triva =>
//            Optional(Newline).As(newline => ));
//
//        public BasicParser<ScanToken, ImmutableList<ScanToken>> LineComment => Mark('/').Then(firstSlash =>
//            Mark('/').Then(secondSlash =>
//                ZeroOrMore(OneOf(AnySpace, AnyMark(), AnyWord())).As(content =>
//                    ImmutableList.Create(firstSlash, secondSlash).AddRange(content)
//                )));
//        
//        /// <summary>
//        /// Creates a parser that resolves a match without consuming the input stream.
//        /// </summary>
//        /// <param name="value">Value the parser will resolve</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TValue">Value type (parser match type)</typeparam>
//        /// <returns>The new parser</returns>
//        public BasicParser<ScanToken, ScanToken> AnyMark()
//        {
//            return Match(input => input.Type == ScanTokenType.Mark)
//                .WithDescription($"{{mark}}");
//        }
//
//        /// <summary>
//        /// Creates a parser that resolves a match without consuming the input stream.
//        /// </summary>
//        /// <param name="value">Value the parser will resolve</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TValue">Value type (parser match type)</typeparam>
//        /// <returns>The new parser</returns>
//        public BasicParser<ScanToken, ScanToken> AnySpace =>
//            Match(input => input.Type == ScanTokenType.Space)
//                .WithDescription($"{{space}}");
//        
//        
//        /// <summary>
//        /// Creates a parser that resolves a match without consuming the input stream.
//        /// </summary>
//        /// <param name="value">Value the parser will resolve</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TValue">Value type (parser match type)</typeparam>
//        /// <returns>The new parser</returns>
//        public BasicParser<ScanToken, ScanToken> Newline()
//        {
//            return Match(input => input.Type == ScanTokenType.Newline)
//                .WithDescription($"{{newline}}");
//        }
//              
//        /// <summary>
//        /// Creates a parser that resolves a match without consuming the input stream.
//        /// </summary>
//        /// <param name="value">Value the parser will resolve</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TValue">Value type (parser match type)</typeparam>
//        /// <returns>The new parser</returns>
//        public BasicParser<ScanToken, ScanToken> AnyWord()
//        {
//            return Match(input => input.Type == ScanTokenType.Word)
//                .WithDescription($"{{word}}");
//        }
//       
//        public BasicParser<ScanToken, ScanToken> Anything()
//        {
//            return Match(input => true)
//                .WithDescription($"{{anything}}");
//        }
//        
//        public BasicParser<ScanToken, ScanToken> Mark(char value)
//        {
//            NotNull(value, nameof(value));
//            var text = value.ToString();
//            return Match(input => input.Type == ScanTokenType.Mark && input.Text == text)
//                .WithDescription($"{{mark({value})}}");
//        }
//      
//        public BasicParser<ScanToken, ScanToken> Keyword(string value)
//        {
//            NotNull(value, nameof(value));
//            return Match(input => input.Type == ScanTokenType.Word && input.Text == value)
//                .WithDescription($"{{keyword({value})}}");
//        }
//    }
//}