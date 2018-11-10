//using System.Collections.Immutable;
//
//namespace Aethon.Glare.Parsing
//{
//    /// <summary>
//    /// Extension methods and static constructors for <see cref="WorkList{T}"/>
//    /// </summary>
//    public static class WorkListExtensions
//    {
//        /// <summary>
//        /// Creates a <see cref="WorkList{T}"/> from a list of matchers.
//        /// </summary>
//        /// <param name="matchers">Matchers to be executed</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <returns>The new work list</returns>
//        public static WorkList<TInput> Work<TInput>(ImmutableList<Matcher<TInput>> matchers) =>
//            new WorkList<TInput>(matchers, ImmutableList<RegisterParser<TInput>>.Empty);
//
//        /// <summary>
//        /// Creates a <see cref="WorkList{T}"/> from a list of matchers.
//        /// </summary>
//        /// <param name="matchers">Matchers to be executed</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <returns>The new work list</returns>
//        public static WorkList<TInput> Work<TInput>(params Matcher<TInput>[] matchers) =>
//            new WorkList<TInput>(ImmutableList.CreateRange(matchers), ImmutableList<RegisterParser<TInput>>.Empty);
//
//        /// <summary>
//        /// Creates a <see cref="WorkList{T}"/> with a parser to be started.
//        /// </summary>
//        /// <param name="parser">Parser to be started</param>
//        /// <param name="resolver">Resolver to invoke when the parser matches</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TMatch">Parse result type</typeparam>
//        /// <returns>The new work list</returns>
//        public static WorkList<TInput> Work<TInput, TMatch>(IParser<TInput, TMatch> parser, Resolver<TInput, TMatch> resolver) =>
//            new WorkList<TInput>(ImmutableList<Matcher<TInput>>.Empty,
//                ImmutableList.Create<RegisterParser<TInput>>(r => r.Register(parser, resolver)));
//
//        /// <summary>
//        /// Creates a new <see cref="WorkList{T}"/> with the combined work of this list and another.
//        /// </summary>
//        /// <param name="this">This work list</param>
//        /// <param name="other">Work list to add</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <returns>The new work list</returns>
//        public static WorkList<TInput> Add<TInput>(this WorkList<TInput> @this, WorkList<TInput> other) =>
//            new WorkList<TInput>(@this.Matchers.AddRange(other.Matchers), @this.Parsers.AddRange(other.Parsers));
//
//        /// <summary>
//        /// Creates a new <see cref="WorkList{T}"/> with the work from this list and an additional parser to start.
//        /// </summary>
//        /// <param name="this">This work list</param>
//        /// <param name="parser">Parser to be started</param>
//        /// <param name="resolver">Resolver to invoke when the parser matches</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <typeparam name="TMatch">Parse result type</typeparam>
//        /// <returns>The new work list</returns>
//        public static WorkList<TInput> Add<TInput, TMatch>(this WorkList<TInput> @this, IParser<TInput, TMatch> parser, Resolver<TInput, TMatch> resolver) =>
//            new WorkList<TInput>(@this.Matchers, @this.Parsers.Add(r => r.Register(parser, resolver)));
//
//        /// <summary>
//        /// Creates a new <see cref="WorkList{T}"/> with the work from this list and additional matchers.
//        /// </summary>
//        /// <param name="this">This work list</param>
//        /// <param name="matchers">Matchers to add</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <returns>The new work list</returns>
//        public static WorkList<TInput> Add<TInput>(this WorkList<TInput> @this, params Matcher<TInput>[] matchers) =>
//            new WorkList<TInput>(@this.Matchers.AddRange(matchers), @this.Parsers);
//
//        /// <summary>
//        /// Determines if a work list is empty (has no matchers or parser registrations).
//        /// </summary>
//        /// <param name="this">This work list</param>
//        /// <typeparam name="TInput">Input element type</typeparam>
//        /// <returns><code>true</code> if the work list has no matchers or parser registrations</returns>
//        public static bool IsEmpty<TInput>(this WorkList<TInput> @this) =>
//            @this.Matchers.IsEmpty && @this.Parsers.IsEmpty;
//    }
//}