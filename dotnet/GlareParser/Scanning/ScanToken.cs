using System;
using static Aethon.Glare.Util.Preconditions;

namespace Aethon.Glare.Scanning
{
    public sealed class ScanToken
    {
        public readonly ScanTokenType Type;
        public readonly string Text;
        public readonly ScanPosition Start;
        public readonly ScanPosition End;

        public ScanToken(ScanTokenType type, string text, ScanPosition start)
        {
            Text = NotNullOrEmpty(text, nameof(text));
            Type = type;
            Start = start;
            switch (type)
            {
                case ScanTokenType.Mark:
                    if (text.Length > 1)
                        throw new ArgumentException("Mark tokens must be a single character", nameof(text));
                    End = start;
                    break;
                case ScanTokenType.Word:
                case ScanTokenType.Space:
                    End = start + (uint) text.Length;
                    break;
                case ScanTokenType.Newline:
                    End = new ScanPosition(start.Absolute + (uint) text.Length, start.Row + 1, 0);
                    break;
                default:
                    throw new ArgumentException($"{type} is not a supported scan token type", nameof(type));
            }
        }

        public override string ToString()
        {
            switch (Type)
            {
                case ScanTokenType.Word:
                    return
                        $"w({Text})";
                case ScanTokenType.Space:
                    return
                        $"s({Text.Replace("\t", "\\t")})";
                case ScanTokenType.Newline:
                    return
                        $"n({Text.Replace("\r", "\\r").Replace("\n", "\\n")})";
                default: // a/k/a case ScanTokenType.Mark
                    return
                        $"m({Text})";
            }
        }

        public static ScanToken Mark(char mark, ScanPosition position) =>
            new ScanToken(ScanTokenType.Mark, mark.ToString(), position);

        public static ScanToken Word(string word, ScanPosition position) =>
            new ScanToken(ScanTokenType.Word, word, position);

        public static ScanToken Space(string space, ScanPosition position) =>
            new ScanToken(ScanTokenType.Space, space, position);

        public static ScanToken Newline(string newline, ScanPosition position) =>
            new ScanToken(ScanTokenType.Newline, newline, position);
    }
}