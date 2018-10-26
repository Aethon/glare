using System;
using System.Collections.Immutable;
using Aethon.Glare.Scanning;
using static Aethon.Glare.Util.Preconditions;

namespace Aethon.Glare.Parsing
{
    public sealed class Token<T>
    {
        public readonly T Type;

        public readonly ImmutableList<ScanToken> LeadingTrivia;

        public readonly ImmutableList<ScanToken> Content;

        public readonly ImmutableList<ScanToken> TrailingTrivia;

        public Token(T type, ImmutableList<ScanToken> leadingTrivia, ImmutableList<ScanToken> content,
            ImmutableList<ScanToken> trailingTrivia)
        {
            Type = type;
            LeadingTrivia = leadingTrivia;
            Content = content;
            TrailingTrivia = trailingTrivia;
        }
    }
}