using System;
using System.Threading.Tasks;

namespace Aethon.Glare.Parsing
{
    /// <inheritdoc />
    /// <summary>
    /// An <see cref="T:Aethon.Glare.Parsing.IParser`1" /> with a description.
    /// </summary>
    /// <typeparam name="TInput">Input element type</typeparam>
    /// <typeparam name="TMatch">Parse result type</typeparam>
    public class BasicParser<TInput, TMatch> : IParser<TInput, TMatch>
    {
        /// <summary>
        /// Function to create the work list to start the parser.
        /// </summary>
        private readonly ParseMethod<TInput, TMatch> _method;
        
        /// <summary>
        /// Description of the parser.
        /// </summary>
        private readonly string _description;

        /// <inheritdoc/>
        public object Key => this;
        
//        /// <inheritdoc/>
        public Task<ParseResult<TInput, TMatch>> Resolve(Input<TInput> input) => _method(input);

        /// <summary>
        /// Creates a new parser from this parser with a new description.
        /// </summary>
        /// <param name="description">Description of the parser</param>
        /// <returns>The new parser</returns>
        public BasicParser<TInput, TMatch> WithDescription(string description) =>
            new BasicParser<TInput, TMatch>(description, _method);

        /// <inheritdoc/>
        public override string ToString() => _description;

        /// <summary>
        /// Creates a new <see cref="T:BasicParser`2"/>
        /// </summary>
        /// <param name="description">Description of the parser</param>
        /// <param name="start">Function to create the work list to start the parser</param>
        public BasicParser(string description, ParseMethod<TInput, TMatch> method)
        {
            _method = method;
            _description = description;
        }
    }
}