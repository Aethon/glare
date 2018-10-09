using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static Aethon.Glare.Parsing.Parsers;

namespace Aethon.Glare.Parsing
{
    public class ParsersUnitTest
    {
        private readonly ITestOutputHelper output;

        public ParsersUnitTest(ITestOutputHelper output)
        {
            this.output = output;
        }
      
        [Fact]
        public void MatchPredicate_WithMatchingStream_Matches()
        {
            var subject = Match<char>(i => i == 'a');
                
            var results = ParseAndDump(subject, "a");

            results.Should().Equal(Char('a'));
        }
        
        [Fact]
        public void MatchPredicate_WithNonMatchingStream_DoesNotMatch()
        {
            var subject = Match<char>(i => i == 'a');
                
            var results = ParseAndDump(subject, "b");

            results.Should().BeEmpty();
        }

        [Fact]
        public void Character_WithMatchingStream_Matches()
        {
            var subject = Character("a-z");
                
            var results = ParseAndDump(subject, "a");

            results.Should().Equal(Char('a'));
        }
        
        [Fact]
        public void Character_WithNonMatchingStream_DoesNotMatch()
        {
            var subject = Character("a-z");
                
            var results = ParseAndDump(subject, "A");

            results.Should().BeEmpty();
        }
        
        [Fact]
        public void Optional_WithMatchingStream_MatchesAndAddsMissingMatch()
        {
            var subject = Optional(Character("a"));
                
            var results = ParseAndDump(subject, "a");

            results.Should().Equal(Char('a'), Missing);
        }
        
        [Fact]
        public void Optional_WithNonMatchingStream_MatchesWithMissingMatch()
        {
            var subject = Optional(Character("a"));
                
            var results = ParseAndDump(subject, "b");

            results.Should().Equal(Missing);
        }
        
        


        private ImmutableList<ParseNode> ParseAndDump(Parser<char> subject, string input)
        {
            var results = subject.Parse(new StringInput(input)).ToImmutableList();
            
            output.WriteLine("Results:");
            foreach (var r in results)
                output.WriteLine($"  {r}");
            output.WriteLine(string.Empty);

            return results;
        }
        
        private static ParsedValue<char> Char(char value) => new ParsedValue<char>(value);
        private static readonly MissingValue Missing = new MissingValue();
    }
}
