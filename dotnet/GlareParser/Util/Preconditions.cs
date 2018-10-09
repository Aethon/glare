using System;

namespace Aethon.Glare.Util
{
    public static class Preconditions
    {
        public static T NotNull<T>(T value, string name)
        {
            if (value == null)
                throw new ArgumentNullException(name);
            return value;
        }
        
        public static T[] NotNullOrEmpty<T>(T[] value, string name)
        {
            if (value == null)
                throw new ArgumentNullException(name);
            if (value.Length == 0)
                throw new ArgumentException("Cannot be empty", name);
            return value;
        }
        
        public static string NotNullOrEmpty(string value, string name)
        {
            if (value == null)
                throw new ArgumentNullException(name);
            if (value.Length == 0)
                throw new ArgumentException("Cannot be empty", name);
            return value;
        }        
    }
}