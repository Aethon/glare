using System;
using static Aethon.Glare.Util.Preconditions;

namespace Aethon.Glare.Scanning
{
    /// <summary>
    /// Describes a token found by the <see cref="CharacterScanner"/>.
    /// </summary>
    public sealed class ScanToken
    {
        /// <summary>
        /// Type of characters found.
        /// </summary>
        public readonly ScanTokenType Type;
        
        /// <summary>
        /// Characters found.
        /// </summary>
        public readonly string Text;
        
        /// <summary>
        /// Position of the first character found.
        /// </summary>
        public readonly ScanPosition Start;
        
        /// <summary>
        /// Position of the last character found.
        /// </summary>
        public readonly ScanPosition End;

        /// <summary>
        /// Creates a new scan token.
        /// </summary>
        /// <param name="type">Type of characters found.</param>
        /// <param name="text">Characters found.</param>
        /// <param name="start">Position of the first character found.</param>
        /// <exception cref="ArgumentException">text was empty or null; type was <see cref="Mark"/> but text was more than one character; type was not understood.</exception>
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

        /// <inheritdoc/>
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

        /// <summary>
        /// Creates a new mark token.
        /// </summary>
        /// <param name="mark">Mark character found.</param>
        /// <param name="position">Position of the mark.</param>
        /// <returns>The new token.</returns>
        public static ScanToken Mark(char mark, ScanPosition position) =>
            new ScanToken(ScanTokenType.Mark, mark.ToString(), position);

        /// <summary>
        /// Creates a new word token.
        /// </summary>
        /// <param name="word">Word characters found.</param>
        /// <param name="position">Position of the word.</param>
        /// <returns>The new token.</returns>
        public static ScanToken Word(string word, ScanPosition position) =>
            new ScanToken(ScanTokenType.Word, word, position);

        /// <summary>
        /// Creates a new space token.
        /// </summary>
        /// <param name="space">Space characters found.</param>
        /// <param name="position">Position of the space(s).</param>
        /// <returns>The new token.</returns>
        public static ScanToken Space(string space, ScanPosition position) =>
            new ScanToken(ScanTokenType.Space, space, position);

        /// <summary>
        /// Creates a new newline token.
        /// </summary>
        /// <param name="newline">Newline character(s) found.</param>
        /// <param name="position">Position of the newline.</param>
        /// <returns>The new token.</returns>
        public static ScanToken Newline(string newline, ScanPosition position) =>
            new ScanToken(ScanTokenType.Newline, newline, position);
    }
}