using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Aethon.Glare.Util;

namespace Aethon.Glare.Parsing
{
    public abstract class ParseResult<E, M> : IEquatable<ParseResult<E, M>>
    {
        public abstract Task<ParseResult<E, M2>> Bind<M2>(
            Func<M, Input<E>, Task<ParseResult<E, M2>>> selector);

        public abstract Task<ParseResult<E, M2>> Bind<M2>(Func<M, IParser<E, M2>> parserFactory);

        public abstract ParseResult<E, M> And(ParseResult<E, M> other);

        public abstract bool Equals(ParseResult<E, M> other);
    }

    public sealed class Match<E, M> : ParseResult<E, M>, IEquatable<Match<E, M>>
    {
        public readonly ImmutableHashSet<Alternative<E, M>> Alternatives;
        private int _hashcode;

        public Match(ImmutableHashSet<Alternative<E, M>> alternatives)
        {
            Alternatives = alternatives;
        }

        public override async Task<ParseResult<E, M2>> Bind<M2>(
            Func<M, Input<E>, Task<ParseResult<E, M2>>> selector)
        {
            var boundTasks = Alternatives.Select(alt => selector(alt.Value, alt.RemainingInput));
            return (await Task.WhenAll(boundTasks))
                .Aggregate((a, r) => a.And(r));
        }

        public override Task<ParseResult<E, M2>> Bind<M2>(
            Func<M, IParser<E, M2>> parserFactory) => Bind((m, r) => parserFactory(m).Resolve(r));

        public override ParseResult<E, M> And(ParseResult<E, M> other)
        {
            switch (other)
            {
                case Match<E, M> match:
                    return new Match<E, M>(Alternatives.Union(match.Alternatives));
                default:
                    return this;
            }
        }

        public override bool Equals(ParseResult<E, M> other) => Equals((object) other);

        public bool Equals(Match<E, M> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Alternatives.SetEquals(other.Alternatives);
        }

        public override bool Equals(object obj) => obj is Match<E, M> other && Equals(other);

        public override int GetHashCode()
        {
            if (_hashcode == 0)
                _hashcode = HashCode.For(Alternatives);
            return _hashcode;
        }

        public override string ToString() => string.Join("\n", Alternatives);
    }

    public class Alternative<E, M> : IEquatable<Alternative<E, M>>
    {
        /// <summary>
        /// Matched value
        /// </summary>
        public readonly M Value;

        public readonly Input<E> RemainingInput;

        /// <summary>
        /// Creates a new Match with no trivia
        /// </summary>
        /// <param name="value">Matched value</param>
        public Alternative(M value, Input<E> remainingInput)
        {
            Value = value;
            RemainingInput = remainingInput;
        }

        public bool Equals(Alternative<E, M> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<M>.Default.Equals(Value, other.Value) &&
                   EqualityComparer<Input<E>>.Default.Equals(RemainingInput, other.RemainingInput);
        }

        public override bool Equals(object obj) => obj is Alternative<E, M> other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<M>.Default.GetHashCode(Value) * 397) ^ RemainingInput.GetHashCode();
            }
        }

        public override string ToString() => $"Matched[{Value}] => {RemainingInput}";
    }

    /// <summary>
    /// Represents a failed parse match
    /// </summary>
    /// <typeparam name="M">Parser match type</typeparam>
    public sealed class Nothing<E, M> : ParseResult<E, M>, IEquatable<Nothing<E, M>>
    {
        public readonly ImmutableHashSet<FailedExpectation> Expectations;
        private int _hashcode = 0;

        public Nothing(ImmutableHashSet<FailedExpectation> expectations)
        {
            Expectations = expectations;
        }

        public Nothing<E, M2> As<M2>() =>
            new Nothing<E, M2>(Expectations);

//        public override Task<T> Select<T>(Func<TMatch, Input<TInput>, Task<T>> match = null,
//            Func<ImmutableList<FailedExpectation>, Task<T>> nothing = null)
//        {
//            return nothing == null ? Task.FromResult(default(T)) : nothing(Expectations);
//        }

        public override Task<ParseResult<E, M2>> Bind<M2>(
            Func<M, Input<E>, Task<ParseResult<E, M2>>> selector) => Task.FromResult((ParseResult<E, M2>)As<M2>());

        public override Task<ParseResult<E, M2>> Bind<M2>(Func<M, IParser<E, M2>> parserFactory) => Task.FromResult((ParseResult<E, M2>)As<M2>());

        public override ParseResult<E, M> And(ParseResult<E, M> other)
        {
            switch (other)
            {
                case Match<E, M> match:
                    return match;
                case Nothing<E, M> nothing:
                    return new Nothing<E, M>(Expectations.Union(nothing.Expectations));
                default:
                    throw new Exception(); // TODO
            }
        }

        public override bool Equals(ParseResult<E, M> other) => Equals((object) other);

        public override string ToString() => string.Join("\n", Expectations);

        public bool Equals(Nothing<E, M> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Expectations.SetEquals(other.Expectations);
        }

        public override bool Equals(object obj) => obj is Nothing<E, M> other && Equals(other);

        public override int GetHashCode() =>
            _hashcode == 0
                ? _hashcode = HashCode.For(Expectations)
                : _hashcode;
    }

    public sealed class FailedExpectation : IEquatable<FailedExpectation>
    {
        public readonly IExpectation Expectation;
        public readonly int Position;
        
        public readonly ImmutableHashSet<FailedExpectation> Causes;

        public FailedExpectation(IExpectation expectation, int position) : this(expectation, position,
            ImmutableHashSet<FailedExpectation>.Empty)
        {
        }

        public FailedExpectation(IExpectation expectation, int position, ImmutableHashSet<FailedExpectation> causes)
        {
            Expectation = expectation;
            Position = position;
            Causes = causes;
        }

        public bool Equals(FailedExpectation other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Expectation.Equals(other.Expectation) && Position == other.Position && Causes.SetEquals(other.Causes);
        }

        public override bool Equals(object obj) =>
            obj is FailedExpectation other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Expectation.GetHashCode();
                hashCode = (hashCode * 397) ^ Position;
                hashCode = (hashCode * 397) ^ HashCode.For(Causes);
                return hashCode;
            }
        }

        public override string ToString() => $"Expected {Expectation.Description} at {Position} (causes: [{string.Join(", ", Causes)}])";
    }
}