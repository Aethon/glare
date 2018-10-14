using System.Linq;
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

            results.Should().BeEquivalentTo('a');
        }

        [Fact]
        public void MatchPredicate_WithNonMatchingStream_DoesNotMatch()
        {
            var subject = Match<char>(i => i == 'a');

            var results = subject.ParseAndDump("b", _output);

            results.Should().BeEmpty();
        }

        [Fact]
        public void Optional_WithMatchingStream_MatchesAndAddsMissingMatch()
        {
            var subject = Optional(Value('a'));

            var results = subject.ParseAndDump("a", _output);

            results.Should().BeEquivalentTo(new Maybe<char>('a'), Maybe<char>.Empty);
        }

        [Fact]
        public void Optional_WithNonMatchingStream_MatchesWithMissingMatch()
        {
            var subject = Optional(Value('a'));

            var results = subject.ParseAndDump("b", _output);

            results.Should().BeEquivalentTo(Maybe<char>.Empty);
        }

        [Fact]
        public void A()
        {
            var subject = OneOrMore(Match<char>(i => i == 'a')).As(c => new string(c.ToArray()));

            var results = subject.ParseAllAndDump("aaa", _output);

            results.Should().BeEquivalentTo("aaa");
        }

        [Fact]
        public void B()
        {
            var subject = Value('a').Then(a => Value('b').Then(b => Value('c').As(c => (a, b, c))));

            var results = subject.ParseAllAndDump("abc", _output);

            results.Should().BeEquivalentTo(('a', 'b', 'c'));
        }

        private abstract class Type
        {
        }

        private sealed class TypeName : Type
        {
            public readonly string Name;

            public TypeName(string name)
            {
                Name = name;
            }

            public override string ToString() => Name;
        }

        private sealed class ListOf : Type
        {
            public readonly Type Type;
            public readonly bool AllowEmpty;

            public ListOf(Type type, bool allowEmpty)
            {
                Type = type;
                AllowEmpty = allowEmpty;
            }

            public override string ToString() => $"({Type}){(AllowEmpty ? '*' : '+')}";
        }

        [Fact]
        public void Recursion()
        {
            // type -> name | oneOrMore | zeroOrMore
            // oneOrMore -> type +
            // zeroOrMore -> type *
            var oneOrMore = Deferred<char, Type>();
            var zeroOrMore = Deferred<char, Type>();
            var name = OneOrMore(Match<char>(char.IsLower)).WithDescription("Name")
                .As(c => (Type) new TypeName(new string(c.ToArray())));
            var type = OneOf(name, oneOrMore, zeroOrMore).WithDescription("Type");
            oneOrMore.Set(
                from t in type
                from _ in Value('+')
                select new ListOf(t, false)
            );
            zeroOrMore.Set(
                from t in type
                from _ in Value('*')
                select new ListOf(t, true)
            );

            type.ParseAndDump("b", _output);

            type.ParseAndDump("b+*", _output);

            type.ParseAllAndDump("number+*", _output);
        }
    }
}