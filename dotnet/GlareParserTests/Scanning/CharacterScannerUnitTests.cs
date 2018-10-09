using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using static Aethon.Glare.Helpers;

namespace Aethon.Glare.Scanning
{
    public class CharacterScannerUnitTests
    {
        [Fact]
        public void Scan_WithNullInput_Throws()
        {
            TestAction(() => CharacterScanner.Scan(null))
                .Should().Throw<ArgumentNullException>().Where(x => x.ParamName == "input");
        }

        [Theory]
        [MemberData(nameof(ScanCases))]
        public void Scan_ReturnsCorrectTokens(string input, List<ScanToken> expectedTokens)
        {
            CharacterScanner.Scan(input)
                .Should().BeEquivalentTo(expectedTokens);
        }

        public static IEnumerable<object[]> ScanCases() =>
            new[]
            {
                Case(""),
                Case("azAZ09_", Word("azAZ09_", 0, 0, 0)),
                Case("abc,def", Word("abc", 0, 0, 0), Mark(',', 3, 0, 3), Word("def", 4, 0, 4)),
                Case(":;", Mark(':', 0, 0, 0), Mark(';', 1, 0, 1)),
                Case(" \t\r", Space(" \t\r", 0, 0, 0)),
                Case("\r\n\n", Newline("\r\n", 0, 0, 0), Newline("\n", 2, 1, 0)),
                Case("\r\r\n", Space("\r", 0, 0, 0), Newline("\r\n", 1, 0, 1)),
            };

        private static object[] Case(string input, params ScanToken[] expectedTokens) =>
            new object[] {input, new List<ScanToken>(expectedTokens)};

        private static ScanToken Word(string text, uint absolutePosition, uint row, uint column) =>
            ScanToken.Word(text, new ScanPosition(absolutePosition, row, column));

        private static ScanToken Mark(char text, uint absolutePosition, uint row, uint column) =>
            ScanToken.Mark(text, new ScanPosition(absolutePosition, row, column));

        private static ScanToken Space(string text, uint absolutePosition, uint row, uint column) =>
            ScanToken.Space(text, new ScanPosition(absolutePosition, row, column));

        private static ScanToken Newline(string text, uint absolutePosition, uint row, uint column) =>
            ScanToken.Newline(text, new ScanPosition(absolutePosition, row, column));
    }
}