using System;
using System.Collections.Generic;
using static Aethon.Glare.Util.Preconditions;

namespace Aethon.Glare.Scanning
{
    /// <summary>
    /// Scans character input, producing a list of "words", marks, spaces and newlines.
    /// </summary>
    public static class CharacterScanner
    {
        /// <summary>
        /// Scans character input, producing a list of "words", marks, spaces and newlines.
        /// </summary>
        /// <param name="input">Input characters</param>
        /// <returns>List of <see cref="ScanToken"/>s</returns>
        public static IEnumerable<ScanToken> Scan(IEnumerable<char> input)
        {
            NotNull((object) input, nameof(input));
            return new Impl().Scan(input);
        }

        private class Impl
        {
            private int _absolutePosition = -1;
            private uint _row;
            private int _column = -1;

            private IEnumerator<char> _enumerator;
            private bool _done;
            private char _candidate;

            private bool Next()
            {
                _absolutePosition++;
                _column++;
                _done = !_enumerator.MoveNext();
                if (_done) return false;

                _candidate = _enumerator.Current;
                return true;
            }

            public IEnumerable<ScanToken> Scan(IEnumerable<char> input)
            {
                using (_enumerator = input.GetEnumerator())
                {
                    Next();
                    while (!_done)
                    {
                        if (IsWordCharacter(_candidate))
                            yield return ReadWord();
                        else if (IsWhitespaceCharacter(_candidate))
                            foreach (var token in ReadWhitespace())
                                yield return token;
                        else
                            yield return ReadMark();
                    }

                    _enumerator = null;
                }
            }

            private ScanToken ReadWord()
            {
                var position = CurrentPosition;

                var word = string.Empty;
                do
                {
                    word += _candidate;
                } while (Next() && IsWordCharacter(_candidate));

                return ScanToken.Word(word, position);
            }

            private IEnumerable<ScanToken> ReadWhitespace()
            {
                var position = CurrentPosition;

                var whitespace = string.Empty;
                var newline = string.Empty;
                do
                {
                    switch (_candidate)
                    {
                        case '\r':
                            if (newline.Length > 0)
                                whitespace += '\r';
                            else
                                newline = "\r";
                            break;
                        case '\n':
                            if (whitespace.Length > 0)
                                yield return ScanToken.Space(whitespace, position);
                            yield return ScanToken.Newline(newline + "\n", position + (uint)whitespace.Length);
                            _column = -1;
                            _row++;
                            Next();
                            yield break;
                        default:
                            whitespace += newline + _candidate;
                            newline = string.Empty;
                            break;
                    }
                } while (Next() && IsWhitespaceCharacter(_candidate));

                yield return ScanToken.Space(whitespace + newline, position);
            }


            private ScanToken ReadMark()
            {
                var mark = ScanToken.Mark(_candidate, CurrentPosition);
                Next();
                return mark;
            }

            private static bool IsWordCharacter(char c) =>
                ('a' <= c && c <= 'z') || ('A' <= c && c <= 'Z') || ('0' <= c && c <= '9') || c == '_';

            private static bool IsWhitespaceCharacter(char c) =>
                c == ' ' || c == '\t' || c == '\r' || c == '\n';

            private ScanPosition CurrentPosition =>
                new ScanPosition((uint)_absolutePosition, _row, (uint)_column);
        }
    }
}