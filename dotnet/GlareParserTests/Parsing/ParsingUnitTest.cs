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
    public abstract class ParsingUnitTest
    {
        protected ITestOutputHelper Out { get; }

        protected ParsingUnitTest(ITestOutputHelper output)
        {
            Out = output;
        }
    }
}