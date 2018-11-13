using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static Aethon.Glare.Parsing.ParseStuff;
using static Aethon.Glare.Parsing.ParserFactory;

namespace Aethon.Glare.Parsing
{
    public class ParsersUnitTests : ParsingUnitTest<char>
    {
        public ParsersUnitTests(ITestOutputHelper output) : base(output)
        {
        }

//        [Fact]
//        public async Task Value_WithMatchingStream_Matches()
//        {
//            var context = ParsingContext.Create("a");
//            var subject = Parsers<char>.Match(i => i == 'a');
//
//            (await subject.ParseAndDump(context.Start, Out))
//                .Should()
//                .Be(SingleMatch('a', context.End));
//        }
//
//        [Fact]
//        public async Task MatchPredicate_WithNonMatchingStream_DoesNotMatch()
//        {
//            var context = ParsingContext.Create("b");
//
//            var subject = Parsers<char>.Match(i => i == 'a');
//
//            (await subject.ParseAndDump(context.Start, Out))
//                .Should()
//                .Be(Nothing<char, char>("a match", 0));
//        }

        [Fact]
        public async Task Optional_WithMatchingStream_MatchesAndAddsMissingMatch()
        {
            var context = ParsingContext.Create("a");
            var subject = Optional(Value('a'));

            var result = (await subject.ParseAndDump(context.Start, Out));

            var expectation = SingleMatch(new Maybe<char>('a'), context.End)
                .And(SingleMatch(Maybe<char>.Empty, context.Start));

            result.Should()
                .Be(expectation);
        }

        [Fact]
        public async Task Optional_WithNonMatchingStream_MatchesWithMissingMatch()
        {
            var context = ParsingContext.Create("b");
            var subject = Optional(Value('a'));

            var results = await subject.ParseAndDump(context.Start, Out);

            results.Should().Be(SingleMatch(Maybe<char>.Empty, context.Start));
        }

        [Fact]
        public async Task A()
        {
            var context = ParsingContext.Create("aaa");
            var subject = OneOrMore(Value('a')).As(c => new string(c.ToArray()));

            var results = await subject.ParseAndDump(context.Start, Out);

            results.Should().Be(Matches(
                Alt("a", context.GetElement(1)),
                Alt("aa", context.GetElement(2)),
                Alt("aaa", context.End)
            ));
        }

        [Fact]
        public async Task A0()
        {
            var context = ParsingContext.Create("aaa");
            var subject = ZeroOrMore(Value('a')).As(c => new string(c.ToArray()));

            var results = await subject.ParseAndDump(context.Start, Out);

            results.Should().Be(Matches(
                Alt("", context.Start),
                Alt("a", context.GetElement(1)),
                Alt("aa", context.GetElement(2)),
                Alt("aaa", context.End)
            ));
        }

        [Fact]
        public async Task A1()
        {
            var context = ParsingContext.Create("abc");
            var subject = OneOrMore(OneOf(Value('a'), Value('b'), Value('c')))
                .Bind(c => End<char>().As(_ => new string(c.ToArray())));

            var results = await subject.ParseAndDump(context.Start, Out);

            results.Should().Be(SingleMatch("abc", context.End));
        }

        [Fact]
        public async Task B()
        {
            var context = ParsingContext.Create("abc");
            var subject = Value('a').Bind(a =>
                Value('b').Bind(b =>
                    Value('c').As(c => (a, b, c))));

            var result = (await subject.ParseAndDump(context.Start, Out));

            result.Should().Be(SingleMatch(('a', 'b', 'c'), context.End));
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

        //   [Fact]
        public async Task Recursion()
        {
            // type -> name | oneOrMore | zeroOrMore
            // oneOrMore -> type +
            // zeroOrMore -> type *
            var oneOrMore = Deferred<char, Type>();
            var zeroOrMore = Deferred<char, Type>();
            var name = OneOrMore(Value('b')) //.WithDescription("Name")
                .As(c => (Type) new TypeName(new string(c.ToArray())));
            var type = OneOf(name, oneOrMore, zeroOrMore); //.WithDescription("Type");
            oneOrMore.Set(
                from t in type
                from _ in Value('+')
                select (Type) new ListOf(t, false)
            );
            zeroOrMore.Set(
                from t in type
                from _ in Value('*')
                select (Type) new ListOf(t, true)
            );

            await type.ParseAndDump(ParsingContext.Create("b").Start, Out);

            await type.ParseAndDump(ParsingContext.Create("b+*").Start, Out);

            await type.ParseAndDump(ParsingContext.Create("bb+*").Start, Out);
        }
    }
}