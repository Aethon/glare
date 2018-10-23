using System.Collections.Immutable;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static Aethon.Glare.Parsing.ParserCombinators;

namespace Aethon.Glare.Parsing
{
    public class SeparatedListParserUnitTests : CommonSeparatedListParserUnitTests
    {
        public SeparatedListParserUnitTests(ITestOutputHelper output) : base(output)
        {
        }

        protected override IParser<char, ImmutableList<char>> GetSubject(IParser<char, char> item,
            IParser<char, char> separator) =>
            SeparatedList(item, separator);

        [Fact]
        public void ParseAll_WithEmptyInput_Matches()
        {
            GetSubject(Parsers.Input('a'), Parsers.Input(':'))
                .ParseAll("")
                .Should().BeEquivalentTo(ImmutableList.Create(ImmutableList<char>.Empty));
        }

        [Fact]
        public void Parse_WithMultipleSeparatedInputs_MatchesAllPrefixes()
        {
            GetSubject(Parsers.Match(char.IsLower), Parsers.Input(':'))
                .Parse("a:b:c").Dump(Out)
                .Should().BeEquivalentTo(
                    ImmutableList<char>.Empty,
                    ImmutableList.Create('a'),
                    ImmutableList.Create('a', 'b'),
                    ImmutableList.Create('a', 'b', 'c')
                );
        }
    }
}