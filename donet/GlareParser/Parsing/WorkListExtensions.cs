using System.Collections.Immutable;

namespace Aethon.Glare.Parsing
{
    /// <summary>
    /// Extension methods and static constructors for <see cref="WorkList{T}"/>
    /// </summary>
    public static class WorkListExtensions
    {
        /// <summary>
        /// Creates a <see cref="WorkList{T}"/> from a list of matchers.
        /// </summary>
        /// <param name="matchers">Matchers to be executed</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The new work list</returns>
        public static WorkList<T> Work<T>(ImmutableList<Matcher<T>> matchers) =>
            new WorkList<T>(matchers, ImmutableList<ParserRegistration<T>>.Empty);

        /// <summary>
        /// Creates a <see cref="WorkList{T}"/> from a list of matchers.
        /// </summary>
        /// <param name="matchers">Matchers to be executed</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The new work list</returns>
        public static WorkList<T> Work<T>(params Matcher<T>[] matchers) =>
            new WorkList<T>(ImmutableList.CreateRange(matchers), ImmutableList<ParserRegistration<T>>.Empty);

        /// <summary>
        /// Creates a <see cref="WorkList{T}"/> with a parser to be started.
        /// </summary>
        /// <param name="parser">Parser to be started</param>
        /// <param name="resolver">Resolver to invoke when the parser matches</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The new work list</returns>
        public static WorkList<T> Work<T>(IParser<T> parser, Resolver<T> resolver) =>
            new WorkList<T>(ImmutableList<Matcher<T>>.Empty,
                ImmutableList.Create(new ParserRegistration<T>(parser, resolver)));

        /// <summary>
        /// Creates a new <see cref="WorkList{T}"/> with the combined work of this list and another.
        /// </summary>
        /// <param name="this">This work list</param>
        /// <param name="other">Work list to add</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The new work list</returns>
        public static WorkList<T> Add<T>(this WorkList<T> @this, WorkList<T> other) =>
            new WorkList<T>(@this.Matchers.AddRange(other.Matchers), @this.Parsers.AddRange(other.Parsers));

        /// <summary>
        /// Creates a new <see cref="WorkList{T}"/> with the work from this list and an additional parser to start.
        /// </summary>
        /// <param name="this">This work list</param>
        /// <param name="parser">Parser to be started</param>
        /// <param name="resolver">Resolver to invoke when the parser matches</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The new work list</returns>
        public static WorkList<T> Add<T>(this WorkList<T> @this, IParser<T> parser, Resolver<T> resolver) =>
            new WorkList<T>(@this.Matchers, @this.Parsers.Add(new ParserRegistration<T>(parser, resolver)));

        /// <summary>
        /// Creates a new <see cref="WorkList{T}"/> with the work from this list and additional matchers.
        /// </summary>
        /// <param name="this">This work list</param>
        /// <param name="matchers">Matchers to add</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The new work list</returns>
        public static WorkList<T> Add<T>(this WorkList<T> @this, params Matcher<T>[] matchers) =>
            new WorkList<T>(@this.Matchers.AddRange(matchers), @this.Parsers);

        /// <summary>
        /// Determines if a work list is empty (has no matchers or parser registrations).
        /// </summary>
        /// <param name="this">This work list</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns><code>true</code> if the work list has no matchers or parser registrations</returns>
        public static bool IsEmpty<T>(this WorkList<T> @this) =>
            @this.Matchers.IsEmpty && @this.Parsers.IsEmpty;
    }
}