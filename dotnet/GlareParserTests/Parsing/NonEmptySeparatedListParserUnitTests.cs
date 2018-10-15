using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static Aethon.Glare.Parsing.Parsers;
using static Aethon.Glare.Parsing.ParserExtensions;

namespace Aethon.Glare.Parsing
{
    public class NonEmptySeparatedListParserUnitTests : CommonSeparatedListParserUnitTests
    {
        public NonEmptySeparatedListParserUnitTests(ITestOutputHelper output) : base(output)
        {
        }

        protected override IParser<char, ImmutableList<char>> GetSubject(IParser<char, char> item,
            IParser<char, char> separator) =>
            NonEmptySeparatedList(item, separator);
        
        [Fact]
        public void ParseAll_WithEmptyInput_DoesNotMatch()
        {
            GetSubject(Input('a'), Input(':'))
                .ParseAll("")
                .Should().BeEmpty();
        }

        [Fact]
        public void Parse_WithMultipleSeparatedInputs_MatchesAllPrefixes()
        {
            GetSubject(Match<char>(char.IsLower), Input(':'))
                .Parse("a:b:c").Dump(Out)
                .Should().BeEquivalentTo(
                    ImmutableList.Create('a'),
                    ImmutableList.Create('a', 'b'),
                    ImmutableList.Create('a', 'b', 'c')
                );
        }
    }
}