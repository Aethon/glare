using System;
using System.Threading.Tasks;
using static Aethon.Glare.Util.Preconditions;

namespace Aethon.Glare.Parsing.Parsers
{
    /// <inheritdoc />
    /// <summary>
    /// An <see cref="T:Aethon.Glare.Parsing.IParser`1" /> with a description.
    /// </summary>
    /// <typeparam name="E">Input element type</typeparam>
    /// <typeparam name="M">Parse result type</typeparam>
    public class BindingParser<E, M1, M2> : ParserBase<E, M2>
    {
        /// <summary>
        /// Function to create the work list to start the parser.
        /// </summary>
        public readonly IParser<E, M1> InitialParser;

        public readonly Func<M1, IParser<E, M2>> NextParserSelector;
        
        /// <inheritdoc/>
        public object Key => this;
        
        /// <inheritdoc/>
        public override async Task<ParseResult<E, M2>> Resolve(Input<E> input)
        {
            return await (await input.Resolve(InitialParser)).Bind(NextParserSelector); // TODO: ensure that the actual failing parser is used as the failing expectation
        }

        /// <summary>
        /// Creates a new <see cref="T:BasicParser`2"/>
        /// </summary>
        /// <param name="description">Description of the parser</param>
        /// <param name="start">Function to create the work list to start the parser</param>
        public BindingParser(IParser<E, M1> initialParser, Func<M1, IParser<E, M2>> nextParserSelector)
        {
            InitialParser = NotNull(initialParser, nameof(initialParser));
            NextParserSelector = NotNull(nextParserSelector, nameof(nextParserSelector));
        }

        public override string Description => $"{InitialParser} binding";
    }
}