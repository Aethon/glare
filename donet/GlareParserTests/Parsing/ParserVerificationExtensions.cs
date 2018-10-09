using System.Collections.Generic;
using System.Collections.Immutable;
using Aethon.Glare.Parsing.ParseTree;
using Xunit.Abstractions;

namespace Aethon.Glare.Parsing
{
    public static class ParserVerificationExtensions
    {
        public static ImmutableList<ParseNode> Dump(this IEnumerable<ParseNode>@this, ITestOutputHelper log)
        {
            var results = @this.ToImmutableList();
            
            log.WriteLine("Results:");
            foreach (var r in results)
                log.WriteLine($"  {r}");
            log.WriteLine(string.Empty);

            return results;
        }

        public static ImmutableList<ParseNode> ParseAndDump<T>(this IParser<T> @this, IEnumerable<T> input,
            ITestOutputHelper log)
            => @this.Parse(input).Dump(log);

        
        public static ImmutableList<ParseNode> ParseAllAndDump<T>(this IParser<T> @this, IEnumerable<T> input,
            ITestOutputHelper log)
            => @this.ParseAll(input).Dump(log);
    }
}
