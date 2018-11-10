using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Aethon.Glare.Scanning;

namespace Aethon.Glare.Parsing
{    
    public interface IInputSource<T> //: IDisposable
    {
        Input<T> Start();
    }
    
    public sealed class GenericInputSource<T> : IInputSource<T>
    {
        private readonly IEnumerable<T> _input;
        private readonly IPackrat<T> _packrat;

        public GenericInputSource(IEnumerable<T> input, IPackrat<T> packrat)
        {
            _input = input;
            _packrat = packrat;
        }

        public Input<T> Start()
        {
            var enumerator = _input.GetEnumerator();
            return GetInput(enumerator);
        }

        private Input<T> GetInput(IEnumerator<T> enumerator)
        {
            if (enumerator.MoveNext())
            {
                return new Element<T>(enumerator.Current, () => GetInput(enumerator), _packrat);
            }

            return new End<T>(_packrat);
        }
    }

    public abstract class Input<T>
    {
        private readonly IPackrat<T> _packrat;

        protected Input(IPackrat<T> packrat)
        {
            _packrat = packrat;
        }

        public Task<ParseResult<T, TMatch>> Resolve<TMatch>(IParser<T, TMatch> parser) =>
            _packrat.Resolve(parser, this);
        
        public abstract TResult Select<TResult>(Func<Element<T>, TResult> element = null,
            Func<End<T>, TResult> end = null);
    }

    public sealed class End<T>: Input<T>
    {
        public End(IPackrat<T> packrat) : base(packrat)
        {
        }

        public override TResult Select<TResult>(Func<Element<T>, TResult> element = null,
            Func<End<T>, TResult> end = null) => end == null ? default : end(this);
    }
    
    public sealed class Element<T> : Input<T>
    {
        private readonly Lazy<Input<T>> _nextFactory;

        public Input<T> Next => _nextFactory.Value;

        public T Value { get; }


        public Element(T value, Func<Input<T>> nextFactory, IPackrat<T> packrat) : base(packrat)
        {
//            _source = source;
            _nextFactory = new Lazy<Input<T>>(nextFactory);
            Value = value;
        }
        
        public override TResult Select<TResult>(Func<Element<T>, TResult> element = null,
            Func<End<T>, TResult> end = null) => element == null ? default : element(this);
    }
    
    /// <summary>
    /// Describes a character position in an input stream.
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Zero-based absolute offset (in unicode characters) in the input stream. 
        /// </summary>
        public readonly uint Absolute;

        /// <summary>
        /// Creates a new scan position.
        /// </summary>
        /// <param name="absolute">Zero-based absolute offset (in unicode characters) in the input stream. </param>
        /// <param name="row">Zero-based effective row in the input stream.</param>
        /// <param name="column">Zero-based effective column in the input stream.</param>
        public Position(uint absolute)
        {
            Absolute = absolute;
        }

        /// <summary>
        /// Adds a positive number of characters to the scan position.
        /// </summary>
        /// <remarks>
        /// This operation advances the absolute and column values, but does not affect the row.
        /// </remarks>
        /// <param name="position">Original position</param>
        /// <param name="positions">Number of characters to advance.</param>
        /// <returns>The new position.</returns>
        public static Position operator +(Position position, uint positions)
        {
            return positions == 0
                ? position
                : new Position(position.Absolute + positions);
        }
    }
}