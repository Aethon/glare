using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Aethon.Glare.Parsing
{
    /// <summary>
    /// Interface to a method that can register parser and a resolver to be started.
    /// </summary>
    /// <typeparam name="E">Input element type</typeparam>
    public interface IPackrat<E>
    {
        /// <summary>
        /// Registers a parser and a resolver to be started
        /// </summary>
        /// <param name="parser">Parser to start</param>
        /// <param name="resolver">Function to call when the parser matches the input</param>
        /// <typeparam name="M">Match type</typeparam>
        /// <returns>List of functions to register additional parsers</returns>
        Task<ParseResult<E, M>> Resolve<M>(IParser<E, M> parser, Input<E> input);
    }

    /// <inheritdoc />
    /// <summary>
    /// Registrar that receives parser registrations, de-dups and starts relevant parsers, and collects created matches.
    /// </summary>
    /// <typeparam name="E">Input element type</typeparam>
    public sealed class Packrat<E>: IPackrat<E>
    {
        // All relays created in this phase, indexed by parser key
        private readonly ConcurrentDictionary<object, Task> _relays = new ConcurrentDictionary<object, Task>();
        
        /// <inheritdoc/>
        public Task<ParseResult<E, M>> Resolve<M>(IParser<E, M> parser, Input<E> input)
        {
            return (Task<ParseResult<E, M>>)_relays.GetOrAdd(new Key(input, parser), key =>
                Start(parser, input)
            );
        }

        private async Task<ParseResult<E, M>> Start<M>(IParser<E, M> parser, Input<E> input)
        {
            var taskFactory = Task.Factory; // TODO: configure            
            return await await taskFactory.StartNew(() => parser.Resolve(input));
        }
        
        private struct Key
        {
            public readonly Input<E> Input;
            public readonly IParser<E> Parser;

            public Key(Input<E> input, IParser<E> parser) : this()
            {
                Input = input;
                Parser = parser;
            }
        }
    }
}