//using System.Collections.Immutable;
//using static Aethon.Glare.Util.Preconditions;
//
//namespace Aethon.Glare.Parsing
//{
//    /// <summary>
//    /// Holds matchers and parsers that should be executed against an input stream.
//    /// </summary>
//    /// <typeparam name="TInput">Input element type</typeparam>
//    public sealed class WorkList<TInput>
//    {
//        /// <summary>
//        /// An empty work list.
//        /// </summary>
//        public static readonly WorkList<TInput> Nothing = new WorkList<TInput>(ImmutableList<Matcher<TInput>>.Empty,
//            ImmutableList<RegisterParser<TInput>>.Empty);
//
//        /// <summary>
//        /// Creates a <see cref="WorkList{T}"/>.
//        /// </summary>
//        /// <param name="matchers">Matchers to be executed</param>
//        /// <param name="parsers">Parsers to be started</param>
//        public WorkList(ImmutableList<Matcher<TInput>> matchers, ImmutableList<RegisterParser<TInput>> parsers)
//        {
//            Matchers = NotNull(matchers, nameof(matchers));
//            Parsers = NotNull(parsers, nameof(parsers));
//        }
//
//        /// <summary>
//        /// Matchers to be executed
//        /// </summary>
//        public ImmutableList<Matcher<TInput>> Matchers { get; }
//        
//        /// <summary>
//        /// Parsers to be started
//        /// </summary>
//        public ImmutableList<RegisterParser<TInput>> Parsers { get; }
//
//        /// <summary>
//        /// Deconstructs the work list.
//        /// </summary>
//        /// <param name="matchers">Matchers</param>
//        /// <param name="parsers">Parsers</param>
//        public void Deconstruct(out ImmutableList<Matcher<TInput>> matchers, out ImmutableList<RegisterParser<TInput>> parsers)
//        {
//            matchers = Matchers;
//            parsers = Parsers;
//        }
//    }
//}