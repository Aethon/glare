using System.Collections.Immutable;

namespace Aethon.GlareParser
{
    public delegate ImmutableList<Matcher<T>> Parser<T>(Finalizer<T> registrar);

    public delegate ImmutableList<Matcher<T>> Matcher<T>(T input);

    public delegate ImmutableList<Matcher<T>> Finalizer<T>(ParseNode match);


    public abstract class ParseNode
    {
        public static readonly ImmutableList<ParseNode> None = ImmutableList<ParseNode>.Empty;
    }

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

    public sealed class ParsedSequence : ParseNode
    {
        public ParsedSequence(ImmutableList<object> items)
        {
            Items = items;
        }

        public ImmutableList<object> Items { get; }

        public override string ToString()
        {
            return $"[Sequence: {string.Join(", ", Items)}]";
        }
    }
}