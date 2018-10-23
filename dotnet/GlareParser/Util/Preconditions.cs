using System;

namespace Aethon.Glare.Util
{
    /// <summary>
    /// Utilities for verifying preconditions in methods.
    /// </summary>
    public static class Preconditions
    {
        /// <summary>
        /// Verifies a value is not null.
        /// </summary>
        /// <param name="value">Value to test.</param>
        /// <param name="name">Name of the value source; typically the <code>nameof</code> the parameter being checked.</param>
        /// <typeparam name="T">Value type.</typeparam>
        /// <returns>The input value.</returns>
        /// <exception cref="ArgumentNullException">The value was null.</exception>
        public static T NotNull<T>(T value, string name)
        {
            if (value == null)
                throw new ArgumentNullException(name);
            return value;
        }
        
        /// <summary>
        /// Verifies that an array is not null and not empty.
        /// </summary>
        /// <param name="value">Array to test.</param>
        /// <param name="name">Name of the array source; typically the <code>nameof</code> the parameter being checked.</param>
        /// <typeparam name="T">Array element type.</typeparam>
        /// <returns>The input value.</returns>
        /// <exception cref="ArgumentNullException">The value was null.</exception>
        /// <exception cref="ArgumentException">The array was empty.</exception>
        public static T[] NotNullOrEmpty<T>(T[] value, string name)
        {
            if (value == null)
                throw new ArgumentNullException(name);
            if (value.Length == 0)
                throw new ArgumentException("Cannot be empty", name);
            return value;
        }
        
        /// <summary>
        /// Verifies that a string is not null and not empty.
        /// </summary>
        /// <param name="value">String to test.</param>
        /// <param name="name">Name of the string source; typically the <code>nameof</code> the parameter being checked.</param>
        /// <returns>The input value.</returns>
        /// <exception cref="ArgumentNullException">The value was null.</exception>
        /// <exception cref="ArgumentException">The string was empty.</exception>
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