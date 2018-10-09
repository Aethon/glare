namespace Aethon.Glare.Parsing.ParseTree
{
    public abstract class ParsedValue : ParseNode
    {
        public object Value { get; }

        protected ParsedValue(object value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"[Value: {Value}]";
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return (obj is ParsedValue pv && pv.Value.Equals(Value));
        }
    }
    
    public sealed class ParsedValue<T> : ParsedValue
    {
        public new T Value => (T) base.Value;

        public ParsedValue(T value) : base(value)
        {
        }
    }
}