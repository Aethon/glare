using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static Aethon.Glare.Parsing.ParseStuff;

namespace Aethon.Glare.Parsing
{
    public class MatchEndParserUnitTests: ParsingUnitTest
    {
        public MatchEndParserUnitTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Resolve_WithElement_DoesNotMatchAndDoesNotConsume()
        {
            var context = ParsingContext.Create("data");
            var subject = Parsers<char>.MatchEnd();

            var result = (await subject.ParseAndDump(context.Start, Out));

            result.Should().Be(Nothing<char, NoValue>("end", 0));
        }
        
        [Fact]
        public async Task Resolve_WithEnd_ReturnsNoValueWithoutConsuming()
        {
            var context = ParsingContext.Create("data");
            var subject = Parsers<char>.MatchEnd();

            var result = await subject.ParseAndDump(context.End, Out);

            result.Should().Be(SingleMatch(NoValue.Instance, context.End));
        }
    }
}