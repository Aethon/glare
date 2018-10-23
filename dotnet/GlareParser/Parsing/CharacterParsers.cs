using System;
using System.Net;
using System.Reflection;
using Aethon.Glare.Scanning;
using static Aethon.Glare.Parsing.WorkListExtensions;
using static Aethon.Glare.Parsing.ParserExtensions;
using static Aethon.Glare.Util.Preconditions;
using static Aethon.Glare.Parsing.ParserCombinators;

namespace Aethon.Glare.Parsing
{
    /// <summary>
    /// Factories for creating general Glare character parsers.
    /// </summary>
    public class ScanParsers: Parsers<ScanToken>
    {
        public BasicParser<ScanToken,ScanToken> LeadingTrivia = ZeroOrMore(OneOf(AnySpace, LineComment, BlockComment, Newline));

        public BasicParser<ScanToken, ScanToken> TrailingTrivia = ZeroOrMore(OneOf(AnySpace, LineComment, BlockComment)).Then(triva =>
            Optional(Newline).As(newline => ));

        public BasicParser<ScanToken, ScanToken> LineComment = Mark('/').Then(firstSlash =>
            Mark('/').Then(secondSlash =>
                ZeroOrMore(OneOf(AnySpace, AnyMark, AnyWord)).As(content => new ScanToken())));
        
        /// <summary>
        /// Creates a parser that resolves a match without consuming the input stream.
        /// </summary>
        /// <param name="value">Value the parser will resolve</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TValue">Value type (parser match type)</typeparam>
        /// <returns>The new parser</returns>
        public BasicParser<ScanToken, ScanToken> AnyMark()
        {
            return Match(input => input.Type == ScanTokenType.Mark)
                .WithDescription($"{{mark}}");
        }

        /// <summary>
        /// Creates a parser that resolves a match without consuming the input stream.
        /// </summary>
        /// <param name="value">Value the parser will resolve</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TValue">Value type (parser match type)</typeparam>
        /// <returns>The new parser</returns>
        public BasicParser<ScanToken, ScanToken> AnySpace =>
            Match(input => input.Type == ScanTokenType.Space)
                .WithDescription($"{{space}}");
        
        
        /// <summary>
        /// Creates a parser that resolves a match without consuming the input stream.
        /// </summary>
        /// <param name="value">Value the parser will resolve</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TValue">Value type (parser match type)</typeparam>
        /// <returns>The new parser</returns>
        public BasicParser<ScanToken, ScanToken> Newline()
        {
            return Match(input => input.Type == ScanTokenType.Newline)
                .WithDescription($"{{newline}}");
        }
              
        /// <summary>
        /// Creates a parser that resolves a match without consuming the input stream.
        /// </summary>
        /// <param name="value">Value the parser will resolve</param>
        /// <typeparam name="TInput">Input element type</typeparam>
        /// <typeparam name="TValue">Value type (parser match type)</typeparam>
        /// <returns>The new parser</returns>
        public BasicParser<ScanToken, ScanToken> AnyWord()
        {
            return Match(input => input.Type == ScanTokenType.Word)
                .WithDescription($"{{word}}");
        }
       
        public BasicParser<ScanToken, ScanToken> Anything()
        {
            return Match(input => true)
                .WithDescription($"{{anything}}");
        }
        
        public BasicParser<ScanToken, ScanToken> Mark(char value)
        {
            NotNull(value, nameof(value));
            var text = value.ToString();
            return Match(input => input.Type == ScanTokenType.Mark && input.Text == text)
                .WithDescription($"{{mark({value})}}");
        }
      
        public BasicParser<ScanToken, ScanToken> Keyword(string value)
        {
            NotNull(value, nameof(value));
            return Match(input => input.Type == ScanTokenType.Word && input.Text == value)
                .WithDescription($"{{keyword({value})}}");
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