using System;
using System.Data.Common;
using System.Diagnostics;

namespace Aethon.Glare.Scanning
{
    /// <summary>
    /// Describes a character position in an input stream.
    /// </summary>
    public struct ScanPosition
    {
        /// <summary>
        /// Zero-based absolute offset (in unicode characters) in the input stream. 
        /// </summary>
        public readonly uint Absolute;
        
        /// <summary>
        /// Zero-based effective row in the input stream.
        /// </summary>
        public readonly uint Row;
        
        /// <summary>
        /// Zero-based effective column in the input stream.
        /// </summary>
        public readonly uint Column;

        /// <summary>
        /// Creates a new scan position.
        /// </summary>
        /// <param name="absolute">Zero-based absolute offset (in unicode characters) in the input stream. </param>
        /// <param name="row">Zero-based effective row in the input stream.</param>
        /// <param name="column">Zero-based effective column in the input stream.</param>
        public ScanPosition(uint absolute, uint row, uint column)
        {
            Absolute = absolute;
            Row = row;
            Column = column;
        }

        /// <summary>
        /// Adds a positive number of characters to the scan position.
        /// </summary>
        /// <remarks>
        /// This operation advances the absolute and column values, but does not affect the row.
        /// </remarks>
        /// <param name="position">Original position</param>
        /// <param name="positions">Number of characters to advance.</param>
        /// <returns>The new position.</returns>
        public static ScanPosition operator +(ScanPosition position, uint positions)
        {
            return positions == 0
                ? position
                : new ScanPosition(position.Absolute + positions, position.Row, position.Column + positions);
        }
    }
}