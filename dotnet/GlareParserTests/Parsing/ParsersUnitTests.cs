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
    public class ParsersUnitTests : ParsingUnitTest
    {
        public ParsersUnitTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void MatchPredicate_WithMatchingStream_Matches()
        {
            var subject = Match<char>(i => i == 'a');

            var results = subject.ParseAndDump("a", Out);

            results.Should().BeEquivalentTo('a');
        }

        [Fact]
        public void MatchPredicate_WithNonMatchingStream_DoesNotMatch()
        {
            var subject = Match<char>(i => i == 'a');

            var results = subject.ParseAndDump("b", Out);

            results.Should().BeEmpty();
        }

        [Fact]
        public void Optional_WithMatchingStream_MatchesAndAddsMissingMatch()
        {
            var subject = Optional(Input('a'));

            var results = subject.ParseAndDump("a", Out);

            results.Should().BeEquivalentTo(new Maybe<char>('a'), Maybe<char>.Empty);
        }

        [Fact]
        public void Optional_WithNonMatchingStream_MatchesWithMissingMatch()
        {
            var subject = Optional(Input('a'));

            var results = subject.ParseAndDump("b", Out);

            results.Should().BeEquivalentTo(Maybe<char>.Empty);
        }

        [Fact]
        public void A()
        {
            var subject = OneOrMore(Match<char>(i => i == 'a')).As(c => new string(c.ToArray()));

            var results = subject.ParseAllAndDump("aaa", Out);

            results.Should().BeEquivalentTo("aaa");
        }

        [Fact]
        public void B()
        {
            var subject = Input('a').Then(a => Input('b').Then(b => Input('c').As(c => (a, b, c))));

            var results = subject.ParseAllAndDump("abc", Out);

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
                from _ in Input('+')
                select new ListOf(t, false)
            );
            zeroOrMore.Set(
                from t in type
                from _ in Input('*')
                select new ListOf(t, true)
            );

            type.ParseAndDump("b", Out);

            type.ParseAndDump("b+*", Out);

            type.ParseAllAndDump("number+*", Out);
        }
    }
}