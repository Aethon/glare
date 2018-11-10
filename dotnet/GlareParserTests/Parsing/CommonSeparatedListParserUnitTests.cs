//using System.Collections.Immutable;
//using FluentAssertions;
//using Xunit;
//using Xunit.Abstractions;
//
//namespace Aethon.Glare.Parsing
//{
//    public abstract class CommonSeparatedListParserUnitTests : ParsingUnitTest
//    {
//        protected CommonSeparatedListParserUnitTests(ITestOutputHelper output) : base(output)
//        {
//        }
//
//        protected abstract IParser<char, ImmutableList<char>> GetSubject(IParser<char, char> item, IParser<char, char> separator);
//
//        [Fact]
//        public void ParseAll_WithSingleInput_Matches()
//        {
//            GetSubject(Parsers.Input('a'), Parsers.Input(':'))
//                .ParseAll("a").Dump(Out)
//                .Should().BeEquivalentTo(ImmutableList.Create(ImmutableList.Create('a')));
//        }
//
//        [Fact]
//        public void ParseAll_WithMultipleSeparatedInputs_Matches()
//        {
//            GetSubject(Parsers.Match(char.IsLower), Parsers.Input(':'))
//                .ParseAll("a:b:c").Dump(Out)
//                .Should().BeEquivalentTo(ImmutableList.Create(ImmutableList.Create('a', 'b', 'c')));
//        }
//
//        [Fact]
//        public void ParseAll_WithLeadingSeparator_DoesNotMatch()
//        {
//            GetSubject(Parsers.Match(char.IsLower), Parsers.Input(':'))
//                .ParseAll(":b:c").Dump(Out)
//                .Should().BeEmpty();
//        }
//
//        [Fact]
//        public void ParseAll_WithTrailingSeparator_DoesNotMatch()
//        {
//            GetSubject(Parsers.Match(char.IsLower), Parsers.Input(':'))
//                .ParseAll("b:c:").Dump(Out)
//                .Should().BeEmpty();
//        }
//        
//        protected static readonly Parsers<char> Parsers = new Parsers<char>();
//    }
//}