//using System.IO;
//using Aethon.Glare.Parsing;
//using FluentAssertions;
//using Xunit;
//
//namespace Aethon.Glare.Syntax
//{
//    public class SyntaxParserTests
//    {
//        public static readonly string GlarePgSyntax =
//            File.ReadAllText("/local-repos/glare-repo/glare/dotnet/GlareParserTests/Syntax/glare-pg-syntax.glrsyn");
//
//        [Fact]
//        public void ParsesGlarePgSyntax()
//        {
//            SyntaxParser.compilationUnit.ParseAll(GlarePgSyntax).Should().HaveCount(1);
//        }
//    }
//}