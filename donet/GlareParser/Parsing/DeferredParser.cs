using System;
using static Aethon.Glare.Util.Preconditions;

namespace Aethon.Glare.Parsing
{
    /// <inheritdoc />
    /// <summary>
    /// Indirect parser that can be initialized after construction to allow recursive dependencies to be expressed.
    /// </summary>
    /// <typeparam name="T">Input element type</typeparam>
    public sealed class DeferredParser<T> : IParser<T>
    {
        /// <summary>
        /// Actual parser to be used
        /// </summary>
        private IParser<T> _parser;

        /// <summary>
        /// Initializes the parser
        /// </summary>
        /// <param name="parser">Actual parser to be used</param>
        /// <exception cref="InvalidOperationException">The actual parser has already been set</exception>
        public void Set(IParser<T> parser)
        {
            if (_parser != null)
                throw new InvalidOperationException("Deferred parser has already been initialized");
            _parser = NotNull(parser, nameof(parser));
        }

        public WorkList<T> Start(Resolver<T> resolver)
        {
            if (_parser == null)
                throw new InvalidOperationException("Deferred parser has not been initialized");
            return _parser.Start(resolver);
        }

        public override string ToString() => 
            _parser == null ? "[unset deferred parser]" : _parser.ToString();
    }
}