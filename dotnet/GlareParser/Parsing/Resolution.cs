namespace Aethon.Glare.Parsing
{
    public abstract class Resolution<TMatch>
    {
        
    }

    public sealed class Match<TMatch> : Resolution<TMatch>
    {
        public readonly TMatch Value;
        public readonly object LeadingTrivia;
        public readonly object TrailingTrivia;

        public Match(TMatch value)
        {
            Value = value;
        }

        public Match(TMatch value, object leadingTrivia, object trailingTrivia)
        {
            Value = value;
            LeadingTrivia = leadingTrivia;
            TrailingTrivia = trailingTrivia;
        }
    }

    public sealed class Failure<TMatch> : Resolution<TMatch>
    {
        public readonly string Expectation;

        public Failure(string expectation)
        {
            Expectation = expectation;
        }
    }
}