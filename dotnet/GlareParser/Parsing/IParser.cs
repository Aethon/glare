using System.Collections.Immutable;
using Aethon.Glare.Scanning;

namespace Aethon.Glare.Parsing
{
    /// <summary>
    /// Definition of a parser that can match an input stream.
    /// </summary>
    /// <typeparam name="TInput">Input element type</typeparam>
    /// <typeparam name="TMatch">Parse result type</typeparam>
    public interface IParser<TInput, TMatch>
    {
        /// <summary>
        /// Key to use to identify this parser.
        /// </summary>
        object Key { get; }

        /// <summary>
        /// Creates the work list to start the parser.
        /// If the parser can match without consuming input, it calls the resolver with the match during this call
        /// and returns the resolved work along with any additional work to be performed.
        /// </summary>
        /// <param name="resolver">Function to invoke when the parser has matched</param>
        /// <returns>The work list to be executed to match the input stream</returns>
        WorkList<TInput> Start(Resolver<TInput, TMatch> resolver);
    }

    /// <summary>
    /// Tests an input element and returns the resulting <see cref="WorkList{T}"/>
    /// </summary>
    /// <param name="input">Input element to examine</param>
    /// <typeparam name="TInput">Input element type</typeparam>
    public delegate WorkList<TInput> Matcher<TInput>(InputElement<TInput> input);

    /// <summary>
    /// Applies a parser match and returns the resulting <see cref="WorkList{T}"/>
    /// </summary>
    /// <param name="match">Match value</param>
    /// <typeparam name="TInput">Input element type</typeparam>
    /// <typeparam name="TMatch">Parse result type</typeparam>
    public delegate WorkList<TInput> Resolver<TInput, TMatch>(Resolution<TInput, TMatch> match);

    /// <summary>
    /// Registers a parser to start matching the input stream.
    /// </summary>
    /// <param name="registrar">Registration that will accept the registration and start the parser</param>
    /// <typeparam name="TInput">Input element type</typeparam>
    public delegate ImmutableList<RegisterParser<TInput>> RegisterParser<TInput>(IParserRegistrar<TInput> registrar);

    public struct InputElement<T>
    {
        public readonly T Value;
        public readonly Position Position;

        public InputElement(T value, Position position)
        {
            Value = value;
            Position = position;
        }
    }

    /// <summary>
    /// Describes a character position in an input stream.
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Zero-based absolute offset (in unicode characters) in the input stream. 
        /// </summary>
        public readonly uint Absolute;

        /// <summary>
        /// Creates a new scan position.
        /// </summary>
        /// <param name="absolute">Zero-based absolute offset (in unicode characters) in the input stream. </param>
        /// <param name="row">Zero-based effective row in the input stream.</param>
        /// <param name="column">Zero-based effective column in the input stream.</param>
        public Position(uint absolute)
        {
            Absolute = absolute;
        }

        /// <summary>
        /// Adds a positive number of characters to the scan position.
        /// </summary>
        /// <remarks>
        /// This operation advances the absolute and column values, but does not affect the row.
        /// </remarks>
        /// <param name="position">Original position</param>
        /// <param name="positions">Number of characters to advance.</param>
        /// <returns>The new position.</returns>
        public static Position operator +(Position position, uint positions)
        {
            return positions == 0
                ? position
                : new Position(position.Absolute + positions);
        }
    }
}