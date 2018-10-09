using System.Collections.Immutable;
using Aethon.Glare.Parsing.ParseTree;
using static Aethon.Glare.Util.Preconditions;

namespace Aethon.Glare.Parsing
{
    /// <summary>
    /// Represents the parsing state after applying an input element to matchers.
    /// </summary>
    /// <typeparam name="T">Input element type</typeparam>
    public sealed class ParseState<T>
    {
        /// <summary>
        /// A state with no matches and no additional work.
        /// </summary>
        public static readonly ParseState<T> Nothing =
            new ParseState<T>(ImmutableList<ParseNode>.Empty, WorkList<T>.Nothing);

        /// <summary>
        /// Creates a new <see cref="ParseState{T}"/>
        /// </summary>
        /// <param name="matches">Matches found by the application of an input element.</param>
        /// <param name="workList">Remaining work to be applied to the remaining input stream.</param>
        public ParseState(ImmutableList<ParseNode> matches, WorkList<T> workList)
        {
            Matches = NotNull(matches, nameof(matches));
            WorkList = NotNull(workList, nameof(workList));
        }

        /// <summary>
        /// Matches found by the application of an input element.
        /// </summary>
        public ImmutableList<ParseNode> Matches { get; }

        /// <summary>
        /// Remaining work to be applied to the remaining input stream.
        /// </summary>
        public WorkList<T> WorkList { get; }

        /// <summary>
        /// Deconstructs the match result
        /// </summary>
        /// <param name="matches">Matches</param>
        /// <param name="workList">WorkList</param>
        public void Deconstruct(out ImmutableList<ParseNode> matches, out WorkList<T> workList)
        {
            matches = Matches;
            workList = WorkList;
        }
    }
}