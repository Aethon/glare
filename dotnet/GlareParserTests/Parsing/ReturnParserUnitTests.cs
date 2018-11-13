using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static Aethon.Glare.Parsing.ParserFactory;

namespace Aethon.Glare.Parsing
{
    public class ReturnParserUnitTests: ParsingUnitTest<char>
    {
        public ReturnParserUnitTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Resolve_WithElement_ReturnsMatchedValueWithoutConsuming()
        {
            var context = ParsingContext.Create("data");
            var subject = Return<char, char>('a');

            var result = (await subject.ParseAndDump(context.Start, Out));

            result.Should().Be(SingleMatch('a', context.Start));
        }
        
        [Fact]
        public async Task Resolve_WithEnd_ReturnsMatchedValueWithoutConsuming()
        {
            var context = ParsingContext.Create("data");
            var subject = Return<char, char>('e');

            var result = await subject.ParseAndDump(context.End, Out);

            result.Should().Be(SingleMatch('e', context.End));
        }
    }
}