using System.Collections.Immutable;
using System.Net;

namespace Aethon.Glare.Parsing
{
    public static class ParseStuff
    {
        public static Alternative<TInput, TMatch> Alt<TInput, TMatch>(TMatch value, Input<TInput> remainingInput) =>
            new Alternative<TInput, TMatch>(value, remainingInput);

        public static ParseResult<TInput, TMatch> SingleMatch<TInput, TMatch>(TMatch value,
            Input<TInput> remainingInput) =>
            new Match<TInput, TMatch>(ImmutableList.Create(Alt(value, remainingInput)));

        public static ParseResult<TInput, TMatch> Matches<TInput, TMatch>(params Alternative<TInput, TMatch>[] alts) =>
            new Match<TInput, TMatch>(ImmutableList.CreateRange(alts));
    }
}