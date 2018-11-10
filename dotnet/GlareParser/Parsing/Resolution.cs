//using System;
//
//namespace Aethon.Glare.Parsing
//{
////    /// <summary>
////    /// Represents the resolution of a parse attempt
////    /// </summary>
////    /// <typeparam name="TMatch">Parser match type</typeparam>
////    public abstract class Alternative<TInput, TMatch>
////    {
////        public abstract TResult Select<TResult>(Func<TMatch, Input<TInput>, TResult> match = null,
////            Func<int, string, TResult> failure = null, Func<TResult> nothing = null);
////
////        public abstract void Apply(Action<TMatch, Input<TInput>> match = null,
////            Action<int, string> failure = null, Action nothing = null);
////    }
//
//    /// <summary>
//    /// Represents a successful parse match
//    /// </summary>
//    /// <typeparam name="TMatch">Parser match type</typeparam>
//    public struct Alternative<TInput, TMatch>
//    {
//        /// <summary>
//        /// Matched value
//        /// </summary>
//        public readonly TMatch Value;
//
//        public readonly Input<TInput> RemainingInput;
//
//        /// <summary>
//        /// Creates a new Match with no trivia
//        /// </summary>
//        /// <param name="value">Matched value</param>
//        public Alternative(TMatch value, Input<TInput> remainingInput)
//        {
//            Value = value;
//            RemainingInput = remainingInput;
//        }
//    }
//
//    /// <summary>
//    /// Represents a failed parse match
//    /// </summary>
//    /// <typeparam name="TMatch">Parser match type</typeparam>
//    public sealed class Nothing<TInput, TMatch> : ParseResult<TInput, TMatch>
//    {
//        /// <summary>
//        /// Actual value matched
//        /// </summary>
//        public readonly int Position;
//
//        /// <summary>
//        /// Description of the expected match
//        /// </summary>
//        public readonly string Expectation;
//
//        /// <summary>
//        /// Creates a new Failure
//        /// </summary>
//        /// <param name="expectation">Description of the expected match</param>
//        public Nothing(string expectation, int position)
//        {
//            Expectation = expectation;
//            Position = position;
//        }
//
//        public Nothing<TInput, TMatch2> As<TMatch2>() =>
//            new Nothing<TInput, TMatch2>(Expectation, Position);
//    }
//}