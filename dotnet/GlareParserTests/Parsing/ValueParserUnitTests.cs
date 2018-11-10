//using System;
//using System.Collections.Immutable;
//using FluentAssertions;
//using Xunit;
//using Xunit.Abstractions;
//
//namespace Aethon.Glare.Parsing
//{
//    public class ValueParserUnitTests: ParsingUnitTest
//    {
//        public ValueParserUnitTests(ITestOutputHelper output) : base(output)
//        {
//        }
//
//        private static readonly WorkList<char> FullWorkList = new WorkList<char>(
//            ImmutableList.Create<Matcher<char>>(input => throw new Exception()),
//            ImmutableList.Create<RegisterParser<char>>(reg => throw new Exception())
//        );
//
//        [Fact]
//        public void Start_ResolvesImmediately_AndReturnsOnlyResolvedWork()
//        {
//            var subject = Parsers.Value("value");
//            Resolution<char, string> result = null;
//            var work = subject.Start(r =>
//            {
//                result = r;
//                return FullWorkList;
//            });
//
//            result.Should().BeEquivalentTo(new Match<char, string>("value"));
//            work.Should().BeEquivalentTo(FullWorkList);
//        }
//        
//        private static readonly Parsers<char> Parsers = new Parsers<char>();
//    }
//}