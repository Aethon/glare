using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;
using static Aethon.Glare.Helpers;

namespace Aethon.Glare.Scanning
{
    public class ScanTokenUnitTests
    {
        [Fact]
        public void New_Mark_Succeeds()
        {
            var result = ScanToken.Mark(':', Start);

            using (new AssertionScope())
            {
                result.Type.Should().Be(ScanTokenType.Mark);
                result.Text.Should().Be(":");
                result.Start.Should().BeEquivalentTo(Start);
                result.End.Should().BeEquivalentTo(Start);
                result.ToString().Should().Be("m(:)");
            }
        }
        
        [Fact]
        public void New_Word_Succeeds()
        {
            var result = ScanToken.Word("abc", Start);

            using (new AssertionScope())
            {
                result.Type.Should().Be(ScanTokenType.Word);
                result.Text.Should().Be("abc");
                result.Start.Should().BeEquivalentTo(Start);
                result.End.Should().BeEquivalentTo(Start + 3);
                result.ToString().Should().Be("w(abc)");
            }
        }
        
        [Fact]
        public void New_Space_Succeeds()
        {
            var result = ScanToken.Space(" \t", Start);

            using (new AssertionScope())
            {
                result.Type.Should().Be(ScanTokenType.Space);
                result.Text.Should().Be(" \t");
                result.Start.Should().BeEquivalentTo(Start);
                result.End.Should().BeEquivalentTo(Start + 2);
                result.ToString().Should().Be("s( \\t)");
            }
        }

                
        [Fact]
        public void New_Newline_Succeeds()
        {
            var result = ScanToken.Newline("\r\n", Start);

            using (new AssertionScope())
            {
                result.Type.Should().Be(ScanTokenType.Newline);
                result.Text.Should().Be("\r\n");
                result.Start.Should().BeEquivalentTo(Start);
                result.End.Should().BeEquivalentTo(new ScanPosition(Start.Absolute + 2, Start.Row + 1, 0));
                result.ToString().Should().Be("n(\\r\\n)");
            }
        }
        
        [Theory]
        [InlineData(ScanTokenType.Mark, "::", "text")]
        [InlineData(ScanTokenType.Mark, "", "text")]
        [InlineData(ScanTokenType.Mark, null, "text")]
        [InlineData(ScanTokenType.Space, "", "text")]
        [InlineData(ScanTokenType.Space, null, "text")]
        [InlineData(ScanTokenType.Word, "", "text")]
        [InlineData(ScanTokenType.Word, null, "text")]
        [InlineData(ScanTokenType.Newline, "", "text")]
        [InlineData(ScanTokenType.Newline, null, "text")]
        [InlineData((ScanTokenType) 100, "t", "type")]
        public void New_WithBadParameters_Throws(ScanTokenType type, string text, string badParameterName)
        {
            TestAction(() => new ScanToken(type, text, Start))
                .Should().Throw<ArgumentException>()
                .Where(x => x.ParamName == badParameterName);
        }

        private static readonly ScanPosition Start = new ScanPosition(6, 2, 3);
    }
}