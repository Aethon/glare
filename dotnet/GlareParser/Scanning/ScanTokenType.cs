namespace Aethon.Glare.Scanning
{
    /// <summary>
    /// Describes the type of a <see cref="ScanToken"/>
    /// </summary>
    public enum ScanTokenType
    {
        /// <summary>
        /// An unbroken string of word characters.
        /// </summary>
        Word,
        
        /// <summary>
        /// A single non-word, non-space, non-newline character.
        /// </summary>
        Mark,
        
        /// <summary>
        /// An unbroken string of spaces, tabs and carriage returns (that are not followed by a line feed).
        /// </summary>
        Space,
        
        /// <summary>
        /// A newline: a single line-feed character or a carriage return and a line feed.
        /// </summary>
        Newline
    }
}