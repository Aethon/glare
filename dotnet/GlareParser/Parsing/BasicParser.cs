using System;

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
        private readonly Func<Resolver<TInput, TMatch>, WorkList<TInput>> _start;
        
        /// <summary>
        /// Description of the parser.
        /// </summary>
        private readonly string _description;

        /// <inheritdoc/>
        public object Key => _start; // use the start function as the key for the parser
        
        /// <inheritdoc/>
        public WorkList<TInput> Start(Resolver<TInput, TMatch> resolver) => _start(resolver);

        /// <summary>
        /// Creates a new parser from this parser with a new description.
        /// </summary>
        /// <param name="description">Description of the parser</param>
        /// <returns>The new parser</returns>
        public BasicParser<TInput, TMatch> WithDescription(string description) =>
            new BasicParser<TInput, TMatch>(description, _start);

        /// <inheritdoc/>
        public override string ToString() => _description;

        /// <summary>
        /// Creates a new <see cref="T:BasicParser`2"/>
        /// </summary>
        /// <param name="description">Description of the parser</param>
        /// <param name="start">Function to create the work list to start the parser</param>
        public BasicParser(string description, Func<Resolver<TInput, TMatch>, WorkList<TInput>> start)
        {
            _start = start;
            _description = description;
        }
    }
}