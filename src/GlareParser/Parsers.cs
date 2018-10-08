using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aethon.GlareParser
{
    public static class Parsers
    {
        public static Parser<char> Character(string characterSet)
        {
            var regex = new Regex($"[{characterSet}]");
            return Match<char>(i => regex.IsMatch(i.ToString()));
        }

        public static Parser<T> Match<T>(T matchValue) =>
            Match<T>(i => matchValue.Equals(i));

        public static Parser<T> Match<T>(Predicate<T> predicate) =>
            f => Matchers<T>(i => predicate(i) ? f(parsedValue(i)) : NoMatch<T>());
        
        public static Parser<T> Optional<T>(Parser<T> inner) =>
            f => inner(f).Add(i => f(new MissingValue()));


        public static Parser<T> Sequence<T>(params Parser<T>[] items) =>
            f =>
            {
                var results = new List<ParseNode>();

                ImmutableList<Matcher<T>> F2(ParseNode r)
                {
                    results.Add(r);
                    return results.Count == items.Length
                        ? f(new ParsedSequence(ImmutableList.CreateRange<object>(results)))
                        : items[results.Count](F2);
                }

                return items[0](F2);
            };

        public static Parser<T> OneOf<T>(params Parser<T>[] options) =>
            f => options.SelectMany(o => o(f)).ToImmutableList();

        public static Parser<T> ZeroOrMore<T>(Parser<T> item) =>
            f => OneOrMore(item)(f).Add(i => f(new ParsedSequence(ImmutableList<object>.Empty)));


        public static Parser<T> OneOrMore<T>(Parser<T> item) =>
            f =>
            {
                var results = new List<ParseNode>();

                ImmutableList<Matcher<T>> F2(ParseNode r)
                {
                    results.Add(r);
                    return f(new ParsedSequence(ImmutableList.CreateRange<object>(results))).AddRange(item(F2));
                }

                return item(F2);
            };

        
        public static IEnumerable<ParseNode> Parse<T>(this Parser<T> @this, Input<T> input)
        {
            var results = new Queue<ParseNode>();
            var remainingMatchers = @this(r =>
            {
                results.Enqueue(r);
                return ImmutableList<Matcher<T>>.Empty;
            });
            while (remainingMatchers.Count > 0 && input.Next())
            {
                remainingMatchers = remainingMatchers.SelectMany(t => t(input.Current)).ToImmutableList();
                while (results.Count > 0)
                    yield return results.Dequeue();
            }
        }
        
        public static IEnumerable<ParseNode> ParseAll<T>(this Parser<T> @this, Input<T> input)
        {
            var results = new Queue<ParseNode>();
            var remainingMatchers = @this(r =>
            {
                results.Enqueue(r);
                return ImmutableList<Matcher<T>>.Empty;
            });
            while (remainingMatchers.Count > 0 && input.Next())
            {
                results.Clear();
                remainingMatchers = remainingMatchers.SelectMany(t => t(input.Current)).ToImmutableList();
            }

            if (!input.End) yield break;
            while (results.Count > 0)
                yield return results.Dequeue();
        }
        
        private static ParseNode parsedValue<T>(T value) => new ParsedValue<T>(value);

        private static ImmutableList<Matcher<T>> NoMatch<T>() => ImmutableList<Matcher<T>>.Empty;
        
        private static ImmutableList<Matcher<T>> Matchers<T>(params Matcher<T>[] matchers) =>
            ImmutableList.CreateRange(matchers);
    }
}