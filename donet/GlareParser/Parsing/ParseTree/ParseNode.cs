using System.Collections.Immutable;

namespace Aethon.Glare.Parsing.ParseTree
{
    public abstract class ParseNode
    {
        public static readonly ImmutableList<ParseNode> None = ImmutableList<ParseNode>.Empty;
    }
}