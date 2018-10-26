using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Aethon.Glare.Parsing
{
    /// <summary>
    /// Interface to a method that can register parser and a resolver to be started.
    /// </summary>
    /// <typeparam name="TInput">Input element type</typeparam>
    public interface IParserRegistrar<TInput>
    {
        /// <summary>
        /// Registers a parser and a resolver to be started
        /// </summary>
        /// <param name="parser">Parser to start</param>
        /// <param name="resolver">Function to call when the parser matches the input</param>
        /// <typeparam name="TMatch">Match type</typeparam>
        /// <returns>List of functions to register additional parsers</returns>
        ImmutableList<RegisterParser<TInput>> Register<TMatch>(IParser<TInput, TMatch> parser, Resolver<TInput, TMatch> resolver);
    }

    /// <inheritdoc />
    /// <summary>
    /// Registrar that receives parser registrations, de-dups and starts relevant parsers, and collects created matches.
    /// </summary>
    /// <typeparam name="TInput">Input element type</typeparam>
    public sealed class ParserRegistrar<TInput>: IParserRegistrar<TInput>
    {
        // All relays created in this phase, indexed by parser key
        private readonly Dictionary<object, MatchRelay> _relays =
            new Dictionary<object, MatchRelay>();

        // All matchers created in this phase
        private readonly List<Matcher<TInput>> _matchers = new List<Matcher<TInput>>();
        
        /// <inheritdoc/>
        public ImmutableList<RegisterParser<TInput>> Register<TMatch>(IParser<TInput, TMatch> parser, Resolver<TInput, TMatch> resolver)
        {
            if (!_relays.TryGetValue(parser.Key, out var relay))
            {
                var newRelay = new MatchRelay<TMatch>(resolver);
                _relays.Add(parser.Key, newRelay);

                var (matchers, newParsers) = parser.Start(newRelay.Resolve);
                _matchers.AddRange(matchers);
                return newParsers;
            }

            relay.Include(resolver);
            return ImmutableList<RegisterParser<TInput>>.Empty;
        }

        // Untyped, abstract version of a relay that will call multiple resolvers for a given match
        private abstract class MatchRelay
        {
            // Includes a resolver in the list of resolvers to call on a match
            public abstract void Include<TMatch>(Resolver<TInput, TMatch> resolver);
        }

        // Typed, concrete relay that will call multiple resolvers for a given match
        private sealed class MatchRelay<TMatch> : MatchRelay
        {
            // Invokes all included resolvers and aggregates the resulting work into a work list
            public WorkList<TInput> Resolve(Resolution<TMatch> match) =>
                _resolvers.Select(f => f(match)).Aggregate((a, b) => a.Add(b));

            // Resolvers to be invoked when the parser matches.
            private readonly List<Resolver<TInput, TMatch>> _resolvers = new List<Resolver<TInput, TMatch>>();

            // Includes a resolver in the list of resolvers to call on a match
            public override void Include<TResolverResult>(Resolver<TInput, TResolverResult> resolver)
            {
                if (typeof(TResolverResult) != typeof(TMatch))
                    throw new ArgumentException($"TODO: good message");
                _resolvers.Add((Resolver<TInput, TMatch>)(object)resolver);
            }

            // Creates a new relay that includes one resolver
            public MatchRelay(Resolver<TInput, TMatch> resolver)
            {
                _resolvers.Add(resolver);
            }
        }

        /// <summary>
        /// Returns the matchers created in current phase and clears the state of the registrar.
        /// </summary>
        /// <returns>List of matchers created in the current phase.</returns>
        public ImmutableList<Matcher<TInput>> GetWork()
        {
            var result = _matchers.ToImmutableList();
            _matchers.Clear();
            _relays.Clear();
            return result;
        }
    }
}