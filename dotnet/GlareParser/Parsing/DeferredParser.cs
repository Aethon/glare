using System;
using System.Threading.Tasks;
using static Aethon.Glare.Util.Preconditions;

namespace Aethon.Glare.Parsing
{
    /// <inheritdoc />
    /// <summary>
    /// Indirect parser that can be initialized after construction to allow recursive dependencies to be expressed.
    /// </summary>
    /// <typeparam name="E">Input element type</typeparam>
    /// <typeparam name="M">Parse result type</typeparam>
    public sealed class DeferredParser<E, M> : IParser<E, M>
    {
        // Actual parser to be used
        private IParser<E, M> _parser;

        /// <summary>
        /// Initializes the parser
        /// </summary>
        /// <param name="parser">Actual parser to be used</param>
        /// <exception cref="InvalidOperationException">The actual parser has already been set</exception>
        public void Set(IParser<E, M> parser)
        {
            if (_parser != null)
                throw new InvalidOperationException("Deferred parser has already been initialized");
            _parser = NotNull(parser, nameof(parser));
        }

        /// <inheritdoc/>
        public object Key => _parser.Key;

        /// <inheritdoc/>
         public Task<ParseResult<E, M>> Resolve(Input<E> input)
        {
            if (_parser == null)
                throw new InvalidOperationException("Deferred parser has not been initialized");
            return _parser.Resolve(input);
        }

        /// <inheritdoc/>
        public override string ToString() => 
            _parser == null ? "[unset deferred parser]" : _parser.ToString();
    }
}