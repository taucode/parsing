using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TauCode.Parsing.Exceptions
{
    [Serializable]
    public class TinyLispException : ParsingException
    {
        public TinyLispException()
        {
        }

        public TinyLispException(string message)
            : base(message)
        {
        }

        public TinyLispException(string message, int? index)
            : base(message)
        {
            this.Index = index;
        }

        public TinyLispException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public int? Index { get; }
    }
}
