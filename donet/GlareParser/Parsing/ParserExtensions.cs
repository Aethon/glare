using System;

namespace Aethon.Glare.Parsing
{
    /// <summary>
    /// Extension methods and static constructors for parsers.
    /// </summary>
    public static partial class ParserExtensions
    {
        /// <summary>
        /// Creates a new <see cref="BasicParser{T}"/>.
        /// </summary>
        /// <param name="start">Function to create the work list to start the parser</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The new parser</returns>
        public static BasicParser<T> Parser<T>(Func<Resolver<T>, WorkList<T>> start) =>
            new BasicParser<T>("{parser}", start);

        /// <summary>
        /// Creates a new <see cref="BasicParser{T}"/> with a description.
        /// </summary>
        /// <param name="description">Description of the parser</param>
        /// <param name="start">Function to create the work list to start the parser</param>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The new parser</returns>
        public static BasicParser<T> Parser<T>(string description, Func<Resolver<T>, WorkList<T>> start) =>
            new BasicParser<T>(description, start);
        
        /// <summary>
        /// Creates a new <see cref="DeferredParser{T}"/>.
        /// </summary>
        /// <typeparam name="T">Input element type</typeparam>
        /// <returns>The deferred parser</returns>
        public static DeferredParser<T> Deferred<T>() => new DeferredParser<T>();
    }
}