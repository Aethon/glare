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
    public class ValueParserUnitTests: ParsingUnitTest
    {
        public ValueParserUnitTests(ITestOutputHelper output) : base(output)
        {
        }

        private static readonly WorkList<char> FullWorkList = new WorkList<char>(
            ImmutableList.Create<Matcher<char>>(input => throw new Exception()),
            ImmutableList.Create<RegisterParser<char>>(reg => throw new Exception())
        );

        [Fact]
        public void Start_ResolvesImmediately_AndReturnsOnlyResolvedWork()
        {
            var subject = Value<char, string>("value");
            string result = null;
            var work = subject.Start(r =>
            {
                result = r;
                return FullWorkList;
            });

            result.Should().Be("value");
            work.Should().BeEquivalentTo(FullWorkList);
        }
    }
}