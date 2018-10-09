using System.Collections.Immutable;
using static Aethon.Glare.Util.Preconditions;

namespace Aethon.Glare.Parsing
{
    /// <summary>
    /// Holds matchers and parsers that should be executed against an input stream.
    /// </summary>
    /// <typeparam name="T">Input element type</typeparam>
    public sealed class WorkList<T>
    {
        /// <summary>
        /// An empty work list.
        /// </summary>
        public static readonly WorkList<T> Nothing = new WorkList<T>(ImmutableList<Matcher<T>>.Empty,
            ImmutableList<ParserRegistration<T>>.Empty);

        /// <summary>
        /// Creates a <see cref="WorkList{T}"./>
        /// </summary>
        /// <param name="matchers">Matchers to be executed</param>
        /// <param name="parsers">Parsers to be started</param>
        public WorkList(ImmutableList<Matcher<T>> matchers, ImmutableList<ParserRegistration<T>> parsers)
        {
            Matchers = NotNull(matchers, nameof(matchers));
            Parsers = NotNull(parsers, nameof(parsers));
        }

        /// <summary>
        /// Matchers to be executed
        /// </summary>
        public ImmutableList<Matcher<T>> Matchers { get; }
        
        /// <summary>
        /// Parsers to be started
        /// </summary>
        public ImmutableList<ParserRegistration<T>> Parsers { get; }

        /// <summary>
        /// Deconstructs the work list.
        /// </summary>
        /// <param name="matchers">Matchers</param>
        /// <param name="parsers">Parsers</param>
        public void Deconstruct(out ImmutableList<Matcher<T>> matchers, out ImmutableList<ParserRegistration<T>> parsers)
        {
            matchers = Matchers;
            parsers = Parsers;
        }
    }
}