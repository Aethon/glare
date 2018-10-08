using System;
using System.Runtime.InteropServices;

namespace Aethon.GlareParser
{
    public abstract class Input<T>
    {
//        public static readonly Point End = new Point();

//        public struct Point
//        {
//            private readonly bool _valued;
//            private readonly T _value;
//            
//            public T Value => _valued ? _value : throw new Exception();
//            public bool AtEnd => !_valued;
//            
//            public Point(T value)
//            {
//                _valued = true;
//                _value = value; // TODO: not null
//            }
//        }

        public bool End { get; private set; }
        public T Current { get; private set; }

        public bool Next()
        {
            (End, Current) = GetNext();
            return !End;
        }

        protected abstract (bool , T next) GetNext();
    }
}