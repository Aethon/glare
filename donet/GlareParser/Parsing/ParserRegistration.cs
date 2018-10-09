using static Aethon.Glare.Util.Preconditions;

namespace Aethon.Glare.Parsing
{
    /// <summary>
    /// Represents an <see cref="IParser"/> that should be started and the <see cref="Resolver"/> that should
    /// invoked when the parser matches.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ParserRegistration<T>
    {
        /// <summary>
        /// The parser to be started.
        /// </summary>
        public IParser<T> Parser { get; }

        /// <summary>
        /// The resolver to be invoked when the parser matches.
        /// </summary>
        public Resolver<T> Resolver { get; }

        /// <summary>
        /// Creates a new <see cref="ParserRegistration{T}"/>
        /// </summary>
        /// <param name="parser">Parser to be started</param>
        /// <param name="resolver">Resolver to invoke when the parser matches</param>
        public ParserRegistration(IParser<T> parser, Resolver<T> resolver)
        {
            Parser = NotNull(parser, nameof(parser));
            Resolver = NotNull(resolver, nameof(resolver));
        }

        /// <summary>
        /// Deconstructs the registration.
        /// </summary>
        /// <param name="parser">Parser</param>
        /// <param name="resolver">Resolver</param>
        public void Deconstruct(out IParser<T> parser, out Resolver<T> resolver)
        {
            parser = Parser;
            resolver = Resolver;
        }
    }
}