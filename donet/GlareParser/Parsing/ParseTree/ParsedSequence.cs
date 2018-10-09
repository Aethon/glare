using System.Collections.Immutable;

namespace Aethon.Glare.Parsing.ParseTree
{
    public sealed class ParsedSequence : ParseNode
    {
        public ParsedSequence(ImmutableList<ParseNode> items)
        {
            Items = items;
        }

        public ImmutableList<ParseNode> Items { get; }

        public override string ToString()
        {
            return $"[Sequence: {string.Join(", ", Items)}]";
        }
    }
}