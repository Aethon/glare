using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Aethon.Glare.Parsing
{
    public class MatchEndParserUnitTests: ParsingUnitTest<char>
    {
        public MatchEndParserUnitTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Resolve_WithElement_DoesNotMatchAndDoesNotConsume()
        {
            var context = ParsingContext.Create("data");
            var subject = ParserFactory.End<char>();

            var result = (await subject.ParseAndDump(context.Start, Out));

            result.Should().Be(Nothing<NoValue>(subject, 0));
        }
        
        [Fact]
        public async Task Resolve_WithEnd_ReturnsNoValueWithoutConsuming()
        {
            var context = ParsingContext.Create("data");
            var subject = ParserFactory.End<char>();

            var result = await subject.ParseAndDump(context.End, Out);

            result.Should().Be(SingleMatch(NoValue.Instance, context.End));
        }
    }
}