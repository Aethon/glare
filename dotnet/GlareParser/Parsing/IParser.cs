using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Aethon.Glare.Scanning;

namespace Aethon.Glare.Parsing
{
//    /// <summary>
//    /// Definition of a parser that can match an input stream.
//    /// </summary>
//    /// <typeparam name="TInput">Input element type</typeparam>
//    /// <typeparam name="TMatch">Parse result type</typeparam>
//    public interface IParser<TInput, TMatch>
//    {
//        /// <summary>
//        /// Key to use to identify this parser.
//        /// </summary>
//        object Key { get; }
//
//        /// <summary>
//        /// Creates the work list to start the parser.
//        /// If the parser can match without consuming input, it calls the resolver with the match during this call
//        /// and returns the resolved work along with any additional work to be performed.
//        /// </summary>
//        /// <param name="resolver">Function to invoke when the parser has matched</param>
//        /// <returns>The work list to be executed to match the input stream</returns>
//        WorkList<TInput> Start(Resolver<TInput, TMatch> resolver);
//
//        Task<ParseResult<TInput, TMatch>> Match(Input<TInput> input);
//    }

    public interface IParser<TInput>
    {
        /// <summary>
        /// Key to use to identify this parser.
        /// </summary>
        object Key { get; }        
    }
    
    public interface IParser<TInput, TMatch> : IParser<TInput>
    {
        ParseResult<TInput, TMatch> Resolve(Input<TInput> input);
    }

    public delegate ParseResult<TInput, TMatch> ParseMethod<TInput, TMatch>(Input<TInput> input);
    
//    /// <summary>
//    /// Tests an input element and returns the resulting <see cref="WorkList{T}"/>
//    /// </summary>
//    /// <param name="input">Input element to examine</param>
//    /// <typeparam name="TInput">Input element type</typeparam>
//    public delegate Resolution<TInput> Matcher<TInput>(InputElement<TInput> input);
//
//    /// <summary>
//    /// Applies a parser match and returns the resulting <see cref="WorkList{T}"/>
//    /// </summary>
//    /// <param name="match">Match value</param>
//    /// <typeparam name="TInput">Input element type</typeparam>
//    /// <typeparam name="TMatch">Parse result type</typeparam>
//    public delegate WorkList<TInput> Resolver<TInput, TMatch>(Resolution<TInput, TMatch> match);

    /// <summary>
    /// Registers a parser to start matching the input stream.
    /// </summary>
    /// <param name="registrar">Registration that will accept the registration and start the parser</param>
    /// <typeparam name="TInput">Input element type</typeparam>
//    public delegate void /*ImmutableList<RegisterParser<TInput>>*/ RegisterParser<TInput>(IPackrat<TInput> registrar);
}