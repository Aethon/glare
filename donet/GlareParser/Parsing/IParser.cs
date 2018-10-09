using Aethon.Glare.Parsing.ParseTree;

namespace Aethon.Glare.Parsing
{
    /// <summary>
    /// Definition of a parser that can match an input stream.
    /// </summary>
    /// <typeparam name="T">Input element type</typeparam>
    public interface IParser<T>
    {
        /// <summary>
        /// Creates the work list to start the parser.
        /// </summary>
        /// <param name="resolver">Function to invoke when the parser has matched</param>
        /// <returns>The work list to be executed to match the input stream</returns>
        WorkList<T> Start(Resolver<T> resolver);
    }
    
    /// <summary>
    /// Tests an input element and returns the resulting <see cref="ParseState{T}"/>
    /// </summary>
    /// <param name="input">Input element to examine</param>
    /// <typeparam name="T">Input element type</typeparam>
    public delegate ParseState<T> Matcher<T>(T input);

    /// <summary>
    /// Applies a parser match and returns the resulting <see cref="ParseState{T}"/>
    /// </summary>
    /// <param name="match">Match value</param>
    /// <typeparam name="T">Input element type</typeparam>
    public delegate ParseState<T> Resolver<T>(ParseNode match);
}