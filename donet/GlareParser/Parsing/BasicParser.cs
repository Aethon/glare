using System;

namespace Aethon.Glare.Parsing
{
    /// <inheritdoc />
    /// <summary>
    /// An <see cref="T:Aethon.Glare.Parsing.IParser`1" /> with a description.
    /// </summary>
    /// <typeparam name="T">Input element type</typeparam>
    public class BasicParser<T> : IParser<T>
    {
        /// <summary>
        /// Function to create the work list to start the parser.
        /// </summary>
        private readonly Func<Resolver<T>, WorkList<T>> _start;
        
        /// <summary>
        /// Description of the parser.
        /// </summary>
        private readonly string _description;

        public WorkList<T> Start(Resolver<T> resolver) => _start(resolver);

        /// <summary>
        /// Creates a new parser from this parser with a new description.
        /// </summary>
        /// <param name="description">Description of the parser</param>
        /// <returns>The new parser</returns>
        public BasicParser<T> WithDescription(string description) =>
            new BasicParser<T>(description, _start);

        public override string ToString() => _description;

        /// <summary>
        /// Creates a new <see cref="BasicParser{T}"/>
        /// </summary>
        /// <param name="description">Description of the parser</param>
        /// <param name="start">Function to create the work list to start the parser</param>
        public BasicParser(string description, Func<Resolver<T>, WorkList<T>> start)
        {
            _start = start;
            _description = description;
        }
    }
}