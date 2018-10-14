using System.Collections.Generic;
using System.Collections.Immutable;
using Xunit.Abstractions;

namespace Aethon.Glare.Parsing
{
    public static class ParserVerificationExtensions
    {
        public static ImmutableList<T> Dump<T>(this IEnumerable<T>@this, ITestOutputHelper log)
        {
            var results = @this.ToImmutableList();
            
            log.WriteLine("Results:");
            foreach (var r in results)
                log.WriteLine($"  {r}");
            log.WriteLine(string.Empty);

            return results;
        }

        public static ImmutableList<TMatch> ParseAndDump<TInput, TMatch>(this IParser<TInput, TMatch> @this, IEnumerable<TInput> input,
            ITestOutputHelper log)
            => @this.Parse(input).Dump(log);

        
        public static ImmutableList<TMatch> ParseAllAndDump<TInput, TMatch>(this IParser<TInput, TMatch> @this, IEnumerable<TInput> input,
            ITestOutputHelper log)
            => @this.ParseAll(input).Dump(log);
    }
}
