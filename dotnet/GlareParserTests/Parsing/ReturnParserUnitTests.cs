using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

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
            var subject = Parsers.Return('a');

            var input = new Element<char>('c', () => throw new Exception("should not call"));

            var result = subject.Resolve(input);

            result.Should().BeEquivalentTo(new ParseMatch<char, char>(ImmutableList.Create(new Match<char, char>('a', input))));
        }
        
        [Fact]
        public async Task Resolve_WithEnd_ReturnsMatchedValueWithoutConsuming()
        {
            var subject = Parsers.Return('e');

            var input = new End<char>();

            var result = subject.Resolve(input);

            result.Should().BeEquivalentTo(new ParseMatch<char, char>(ImmutableList.Create(new Match<char, char>('e', input))));
        }
        
        private static readonly Parsers<char> Parsers = new Parsers<char>();
    }
}