namespace Aethon.Glare.Parsing
{
    /// <summary>
    /// Represents the resolution of a parse attempt
    /// </summary>
    /// <typeparam name="TMatch">Parser match type</typeparam>
    public abstract class Resolution<TMatch>
    {
        
    }

    /// <summary>
    /// Represents a successful parse match
    /// </summary>
    /// <typeparam name="TMatch">Parser match type</typeparam>
    public sealed class Match<TMatch> : Resolution<TMatch>
    {
        /// <summary>
        /// Matched value
        /// </summary>
        public readonly TMatch Value;
        
        /// <summary>
        /// Leading trivia matched
        /// </summary>
        public readonly object LeadingTrivia;
        
        /// <summary>
        /// Trailing trivia matched
        /// </summary>
        public readonly object TrailingTrivia;

        /// <summary>
        /// Creates a new Match with no trivia
        /// </summary>
        /// <param name="value">Matched value</param>
        public Match(TMatch value)
        {
            Value = value;
        }

        /// <summary>
        /// Creates a new Match
        /// </summary>
        /// <param name="value">Matched value</param>
        /// <param name="leadingTrivia">Matched leading trivia</param>
        /// <param name="trailingTrivia">Matched trailing trivia</param>
        public Match(TMatch value, object leadingTrivia, object trailingTrivia)
        {
            Value = value;
            LeadingTrivia = leadingTrivia;
            TrailingTrivia = trailingTrivia;
        }
    }

    /// <summary>
    /// Represents a failed parse match
    /// </summary>
    /// <typeparam name="TMatch">Parser match type</typeparam>
    public sealed class Failure<TMatch> : Resolution<TMatch>
    {
        /// <summary>
        /// Description of the expected match
        /// </summary>
        public readonly string Expectation;

        /// <summary>
        /// Creates a new Failure
        /// </summary>
        /// <param name="expectation">Description of the expected match</param>
        public Failure(string expectation)
        {
            Expectation = expectation;
        }
    }
}