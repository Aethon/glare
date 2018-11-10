using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static Aethon.Glare.Parsing.ParseStuff;

namespace Aethon.Glare.Parsing
{
    public class ReturnParserUnitTests: ParsingUnitTest
    {
        public ReturnParserUnitTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Resolve_WithElement_ReturnsMatchedValueWithoutConsuming()
        {
            var input = new Element<char>('c', () => throw new Exception("should not call"), new Packrat<char>());
            var subject = Parsers.Return('a');

            var result = await subject.Resolve(input);

            result.Should().BeEquivalentTo(SingleMatch('a', input));
        }
        
        [Fact]
        public async Task Resolve_WithEnd_ReturnsMatchedValueWithoutConsuming()
        {
            var input = new End<char>(new Packrat<char>());
            var subject = Parsers.Return('e');

            var result = await subject.Resolve(input);

            result.Should().BeEquivalentTo(SingleMatch('e', input));
        }
        
        private static readonly Parsers<char> Parsers = new Parsers<char>();
    }
}