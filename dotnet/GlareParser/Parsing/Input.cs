using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Aethon.Glare.Util;
using static Aethon.Glare.Util.Preconditions;

namespace Aethon.Glare.Parsing
{
    public sealed class InputSource<E>
    {
        public int ElementCount => _sourceElements.Count;

        private readonly ImmutableList<E> _sourceElements;

        public InputSource(ImmutableList<E> sourceElements)
        {
            _sourceElements = NotNull(sourceElements, nameof(sourceElements));
        }

        public E this[int index] => _sourceElements[index];

        public override string ToString() => $"InputSource<{typeof(E).Name}>[{_sourceElements.Count} elements]";
    }


    public sealed class ParsingContext<E>
    {
        private readonly InputSource<E> _source;
        private readonly IPackrat<E> _packrat;

        public ParsingContext(InputSource<E> source, IPackrat<E> packrat)
        {
            _source = NotNull(source, nameof(source));
            _packrat = NotNull(packrat, nameof(packrat));
        }

        public Task<ParseResult<E, M>> Resolve<M>(IParser<E, M> parser, Input<E> input) =>
            _packrat.Resolve(parser, input);

        public Input<E> Start => GetElement(0);
        public Input<E> End => new End<E>(_source.ElementCount, this);

        public Input<E> GetElement(int position) =>
            _source.ElementCount > position
                ? new Element<E>(position, this)
                : End;

        public E GetElementValue(int position) => _source[position];

        public override string ToString() => $"ParsingContext[source: {_source}, packrat: {_packrat}]";
    }


    public static class ParsingContext
    {
        public static ParsingContext<char> Create(string source) =>
            new ParsingContext<char>(new InputSource<char>(ImmutableList.CreateRange(source)), new Packrat<char>());
    }

    public abstract class Input<E> : IEquatable<Input<E>>
    {
        public readonly int Position;

        protected readonly ParsingContext<E> Context;

        protected Input(int position, ParsingContext<E> context)
        {
            Position = position;
            Context = context;
        }

        public Task<ParseResult<E, TMatch>> Resolve<TMatch>(IParser<E, TMatch> parser) =>
            Context.Resolve(parser, this);

        public abstract T Select<T>(Func<Element<E>, T> element = null, Func<End<E>, T> end = null);

        public bool Equals(Input<E> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            return Position == other.Position && ReferenceEquals(Context, other.Context);
        }

        public override bool Equals(object obj) =>
            (obj is Input<E> other) && Equals(other);

        public override int GetHashCode() => HashCode.Combined(Position.GetHashCode(), Context.GetHashCode());
    }

    public sealed class End<E> : Input<E>, IEquatable<End<E>>
    {
        public End(int position, ParsingContext<E> context) : base(position, context)
        {
        }

        public override T Select<T>(Func<Element<E>, T> element = null, Func<End<E>, T> end = null) =>
            end == null
                ? default
                : end(this);

        public override string ToString() => $"End@{Position}";

        public bool Equals(End<E> other) => base.Equals(other);
    }

    public sealed class Element<E> : Input<E>, IEquatable<Element<E>>
    {
        public Input<E> Next() => Context.GetElement(Position + 1);

        public E Value => Context.GetElementValue(Position);

        private int _hashcode;

        public Element(int position, ParsingContext<E> context) : base(position, context)
        {
        }

        public override T Select<T>(Func<Element<E>, T> element = null, Func<End<E>, T> end = null) =>
            element == null
                ? default
                : element(this);

        public override string ToString() => $"[{Value}]@{Position}";

        public bool Equals(Element<E> other) => base.Equals(other);
    }
}