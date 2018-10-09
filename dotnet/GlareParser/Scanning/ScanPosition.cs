using System;
using System.Data.Common;
using System.Diagnostics;

namespace Aethon.Glare.Scanning
{
    public struct ScanPosition
    {
        public readonly uint Absolute;
        public readonly uint Row;
        public readonly uint Column;

        public ScanPosition(uint absolute, uint row, uint column)
        {
            Absolute = absolute;
            Row = row;
            Column = column;
        }

        public static ScanPosition operator +(ScanPosition position, uint positions)
        {
            return positions == 0
                ? position
                : new ScanPosition(position.Absolute + positions, position.Row, position.Column + positions);
        }
    }
}