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

    public interface IExpectation
    {
        string Description { get; }
    }
    
    public interface IParser<E>: IExpectation
    {
        /// <summary>
        /// Key to use to identify this parser.
        /// </summary>
      //  object Key { get; }        
    }
    
    public interface IParser<E, M> : IParser<E>
    {
        Task<ParseResult<E, M>> Resolve(Input<E> input);
    }

    public delegate Task<ParseResult<E, M>> ParseMethod<E, M>(Input<E> input);
}