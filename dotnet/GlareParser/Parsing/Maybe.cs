using System;
using System.Collections.Generic;
using static Aethon.Glare.Util.Preconditions;

namespace Aethon.Glare.Parsing
{
    public struct NoValue
    {
        public static readonly NoValue Instance = new NoValue();
    }
    
    /// <summary>
    /// Weak implementation of an optional value; to be improved
    /// </summary>
    /// <typeparam name="T">Contained value type</typeparam>
    public struct Maybe<T> : IEquatable<Maybe<T>>
    {
        /// <summary>
        /// A Maybe with no value.
        /// </summary>
        public static readonly Maybe<T> Empty = new Maybe<T>();

        private readonly T _value;

        /// <summary>
        /// Indicates whether the Maybe contains a value.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Gets the value of the Maybe or throws an exception if the Maybe has no value.
        /// </summary>
        /// <remarks>
        /// TODO: formalize the exception and provide overloads to customize
        /// </remarks>
        /// <exception cref="Exception">The Maybe had no value.</exception>
        public T ValueOrThrow
        {
            get
            {
                if (!HasValue)
                    throw new Exception("No Value");
                return _value;
            }
        }

        /// <summary>
        /// Creates a new Maybe from a value.
        /// </summary>
        /// <param name="value">Value of the new Maybe</param>
        public Maybe(T value)
        {
            HasValue = true;
            _value = NotNull(value, nameof(value));
        }

        public override string ToString() => HasValue
            ? $"Value[{_value}]"
            : "Empty";

        public bool Equals(Maybe<T> other)
        {
            return EqualityComparer<T>.Default.Equals(_value, other._value) && HasValue == other.HasValue;
        }

        public override bool Equals(object obj)
        {
            return obj is Maybe<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(_value);
        }
    }
}