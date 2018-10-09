using Aethon.Glare.Parsing.ParseTree;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static Aethon.Glare.Parsing.Parsers;
using static Aethon.Glare.Parsing.ParserExtensions;

namespace Aethon.Glare.Parsing
{
    public class ParsersUnitVerifications
    {
        private readonly ITestOutputHelper _output;

        public ParsersUnitVerifications(ITestOutputHelper output)
        {
            _output = output;
        }
      
        [Fact]
        public void MatchPredicate_WithMatchingStream_Matches()
        {
            var subject = Match<char>(i => i == 'a');
                
            var results = subject.ParseAndDump("a", _output);

            results.Should().BeEquivalentTo(Char('a'));
        }
        
        [Fact]
        public void MatchPredicate_WithNonMatchingStream_DoesNotMatch()
        {
            var subject = Match<char>(i => i == 'a');
                
            var results = subject.ParseAndDump("b", _output);

            results.Should().BeEmpty();
        }

//        [Fact]
//        public void Character_WithMatchingStream_Matches()
//        {
//            var subject = Character("a-z");
//                
//            var results = subject.ParseAndDump("a", _output);
//
//            results.Should().BeEquivalentTo(Char('a'));
//        }
//        
//        [Fact]
//        public void Character_WithNonMatchingStream_DoesNotMatch()
//        {
//            var subject = Character("a-z");
//                
//            var results = subject.ParseAndDump("A", _output);
//
//            results.Should().BeEmpty();
//        }
        
        [Fact]
        public void Optional_WithMatchingStream_MatchesAndAddsMissingMatch()
        {
            var subject = Optional(Value('a'));
                
            var results = subject.ParseAndDump("a", _output);

            results.Should().BeEquivalentTo(Char('a'), Missing);
        }
        
        [Fact]
        public void Optional_WithNonMatchingStream_MatchesWithMissingMatch()
        {
            var subject = Optional(Value('a'));
                
            var results = subject.ParseAndDump("b", _output);

            results.Should().BeEquivalentTo(Missing);
        }
        
        [Fact]
        public void Recursion()
        {
            // type -> name | oneOrMore | zeroOrMore
            // oneOrMore -> type +
            // zeroOrMore -> type *
            var oneOrMore = Deferred<char>();
            var zeroOrMore = Deferred<char>();
            var name = Value('b').WithDescription("Name");
            var type = OneOf(name,  oneOrMore, zeroOrMore).WithDescription("Type");
            oneOrMore.Set(Sequence(type, Value('+')).WithDescription("Type+"));
            zeroOrMore.Set(Sequence(type, Value('*')).WithDescription("Type*"));

            type.ParseAndDump("b", _output);

            ZeroOrMore(type).ParseAndDump("b+*b**", _output);

            ZeroOrMore(type).ParseAllAndDump("b+*b**", _output);
        }        
        
        private static ParsedValue<char> Char(char value) => new ParsedValue<char>(value);
        private static readonly MissingValue Missing = new MissingValue();
    }
}
