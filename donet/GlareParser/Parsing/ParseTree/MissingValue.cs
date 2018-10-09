namespace Aethon.Glare.Parsing.ParseTree
{
    public sealed class MissingValue : ParseNode
    {
        public override string ToString()
        {
            return "[Missing Value]";
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj is MissingValue;
        }
    }
}