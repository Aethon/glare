using System;

namespace Aethon.Glare.Parsing
{
    /// <summary>
    /// Weak implementation of an optional value; to be improved
    /// </summary>
    /// <typeparam name="T">Contained value type</typeparam>
    public struct Maybe<T>
    {
        public static readonly Maybe<T> Empty = new Maybe<T>();
        
        private readonly T _value;
        
        public bool HasValue { get; }

        public T ValueOrThrow
        {
            get
            {
                if (!HasValue)
                    throw new Exception("No Value");
                return _value;
            }
        }

        public Maybe(T value)
        {
            HasValue = true;
            _value = value;
        }
    }
}