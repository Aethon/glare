using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Aethon.Glare.Parsing
{
    public static class ParserVerificationExtensions
    {
        public static ParseResult<T, TMatch> Dump<T, TMatch>(this ParseResult<T, TMatch> @this, ITestOutputHelper log)
        {
            log.WriteLine(@this.ToString());
            return @this;
        }

//        public static async Task<ParseResult<char, TMatch>> ParseAndDump<TMatch>(
//            this IParser<char, TMatch> @this,
//            ParsingContext<char> context,
//            ITestOutputHelper log)
//            => (await @this.Resolve(context.Start)).Dump(log);

        public static async Task<ParseResult<TInput, TMatch>> ParseAndDump<TInput, TMatch>(
            this IParser<TInput, TMatch> @this,
            Input<TInput> input,
            ITestOutputHelper log)
            => (await @this.Resolve(input)).Dump(log);
    }
}