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
    /// <typeparam name="TInput">Input element type</typeparam>
    public interface IPackrat<TInput>
    {
        /// <summary>
        /// Registers a parser and a resolver to be started
        /// </summary>
        /// <param name="parser">Parser to start</param>
        /// <param name="resolver">Function to call when the parser matches the input</param>
        /// <typeparam name="TMatch">Match type</typeparam>
        /// <returns>List of functions to register additional parsers</returns>
        Task<ParseResult<TInput, TMatch>> Resolve<TMatch>(IParser<TInput, TMatch> parser, Input<TInput> input);
    }

    /// <inheritdoc />
    /// <summary>
    /// Registrar that receives parser registrations, de-dups and starts relevant parsers, and collects created matches.
    /// </summary>
    /// <typeparam name="TInput">Input element type</typeparam>
    public sealed class Packrat<TInput>: IPackrat<TInput>
    {
        // All relays created in this phase, indexed by parser key
        private readonly ConcurrentDictionary<object, Task> _relays = new ConcurrentDictionary<object, Task>();
        
        /// <inheritdoc/>
        public Task<ParseResult<TInput, TMatch>> Resolve<TMatch>(IParser<TInput, TMatch> parser, Input<TInput> input)
        {
            var taskFactory = Task.Factory; // TODO: configure
            return (Task<ParseResult<TInput, TMatch>>)_relays.GetOrAdd(new Key(input, parser), key => taskFactory.StartNew(() => parser.Resolve(input)));
        }

        private struct Key
        {
            public readonly Input<TInput> Input;
            public readonly IParser<TInput> Parser;

            public Key(Input<TInput> input, IParser<TInput> parser) : this()
            {
                Input = input;
                Parser = parser;
            }
        }
    }
}