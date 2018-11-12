using System;
using System.Threading.Tasks;

namespace Aethon.Glare.Parsing
{
    /// <inheritdoc />
    /// <summary>
    /// An <see cref="T:Aethon.Glare.Parsing.IParser`1" /> with a description.
    /// </summary>
    /// <typeparam name="E">Input element type</typeparam>
    /// <typeparam name="M">Parse result type</typeparam>
    public class BasicParser<E, M> : IParser<E, M>
    {
        /// <summary>
        /// Function to create the work list to start the parser.
        /// </summary>
        private readonly ParseMethod<E, M> _method;
        
        /// <summary>
        /// Description of the parser.
        /// </summary>
        private readonly string _description;

        /// <inheritdoc/>
        public object Key => this;
        
//        /// <inheritdoc/>
        public Task<ParseResult<E, M>> Resolve(Input<E> input) => _method(input);

        /// <summary>
        /// Creates a new parser from this parser with a new description.
        /// </summary>
        /// <param name="description">Description of the parser</param>
        /// <returns>The new parser</returns>
        public BasicParser<E, M> WithDescription(string description) =>
            new BasicParser<E, M>(description, _method);

        /// <inheritdoc/>
        public override string ToString() => _description;

        /// <summary>
        /// Creates a new <see cref="T:BasicParser`2"/>
        /// </summary>
        /// <param name="description">Description of the parser</param>
        /// <param name="start">Function to create the work list to start the parser</param>
        public BasicParser(string description, ParseMethod<E, M> method)
        {
            _method = method;
            _description = description;
        }
    }
}