//using System;
//using static Aethon.Glare.Util.Preconditions;
//
//namespace Aethon.Glare.Parsing
//{
//    /// <inheritdoc />
//    /// <summary>
//    /// Indirect parser that can be initialized after construction to allow recursive dependencies to be expressed.
//    /// </summary>
//    /// <typeparam name="TInput">Input element type</typeparam>
//    /// <typeparam name="TMatch">Parse result type</typeparam>
//    public sealed class DeferredParser<TInput, TMatch> : IParser<TInput, TMatch>
//    {
//        // Actual parser to be used
//        private IParser<TInput, TMatch> _parser;
//
//        /// <summary>
//        /// Initializes the parser
//        /// </summary>
//        /// <param name="parser">Actual parser to be used</param>
//        /// <exception cref="InvalidOperationException">The actual parser has already been set</exception>
//        public void Set(IParser<TInput, TMatch> parser)
//        {
//            if (_parser != null)
//                throw new InvalidOperationException("Deferred parser has already been initialized");
//            _parser = NotNull(parser, nameof(parser));
//        }
//
//        /// <inheritdoc/>
//        public object Key => _parser.Key;
//
//        /// <inheritdoc/>
//        public WorkList<TInput> Start(Resolver<TInput, TMatch> resolver)
//        {
//            if (_parser == null)
//                throw new InvalidOperationException("Deferred parser has not been initialized");
//            return _parser.Start(resolver);
//        }
//
//        /// <inheritdoc/>
//        public override string ToString() => 
//            _parser == null ? "[unset deferred parser]" : _parser.ToString();
//    }
//}