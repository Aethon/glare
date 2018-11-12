using System.Collections.Generic;

namespace Aethon.Glare.Util
{
    public static class HashCode
    {
        // never returns 0, so zero can be used as a sentinel
        public static int For<T>(IEnumerable<T> subject)
        {
            var hc = 0;
            foreach (var alt in subject)
            {
                hc ^= alt.GetHashCode();
                hc = (hc << 7) | (hc >> 25);
            }

            return hc == 0 ? 1 : hc;
        }

        public static int Combined(params int[] hashCodes) => For(hashCodes);
    }
}