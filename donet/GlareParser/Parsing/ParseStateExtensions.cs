using System.Collections.Immutable;
using Aethon.Glare.Parsing.ParseTree;

namespace Aethon.Glare.Parsing
{
    /// <summary>
    /// Extension methods and static constructors for <see cref="ParseState{T}"/>
    /// </summary>
    public static class ParseStateExtensions
    {
        /// <summary>
        /// Creates a <see cref="ParseState{T}"/> with a match and no remaining work.
        /// </summary>
        /// <param name="match">Match found by the application of an input element.</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The new state</returns>
        public static ParseState<T> State<T>(ParseNode match) =>
            new ParseState<T>(ImmutableList.Create(match), WorkList<T>.Nothing);

        /// <summary>
        /// Creates a <see cref="ParseState{T}"/> remaining work and no match.
        /// </summary>
        /// <param name="workList">Remaining work</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The new state</returns>
        public static ParseState<T> State<T>(WorkList<T> workList) =>
            new ParseState<T>(ImmutableList<ParseNode>.Empty, workList);

        /// <summary>
        /// Creates a new <see cref="ParseState{T}"/> combining this state and another.
        /// </summary>
        /// <param name="this">This state</param>
        /// <param name="other">State to add</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The new state</returns>
        public static ParseState<T> Add<T>(this ParseState<T> @this, ParseState<T> other) =>
            new ParseState<T>(@this.Matches.AddRange(other.Matches), @this.WorkList.Add(other.WorkList));

        /// <summary>
        /// Creates a new <see cref="ParseState{T}"/> combining this state with a parser to start.
        /// </summary>
        /// <param name="this">This state</param>
        /// <param name="parser">Parser to add</param>
        /// <param name="resolver">Resolver to invoke when the parser matches</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The new state</returns>
        public static ParseState<T> Add<T>(this ParseState<T> @this, IParser<T> parser, Resolver<T> resolver) =>
            new ParseState<T>(@this.Matches, @this.WorkList.Add(parser, resolver));

        /// <summary>
        /// Creates a new <see cref="ParseState{T}"/> combining this state with a list of matchers.
        /// </summary>
        /// <param name="this">This state</param>
        /// <param name="matchers">Matchers to add.</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The new state</returns>
        public static ParseState<T> Add<T>(this ParseState<T> @this, params Matcher<T>[] matchers) =>
            new ParseState<T>(@this.Matches, @this.WorkList.Add(matchers));

        /// <summary>
        /// Creates a new <see cref="ParseState{T}"/> combining this state with a work list.
        /// </summary>
        /// <param name="this">This state</param>
        /// <param name="work">Work list to add</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The new state</returns>
        public static ParseState<T> Add<T>(this ParseState<T> @this, WorkList<T> work) =>
            new ParseState<T>(@this.Matches, @this.WorkList.Add(work));
    }
}