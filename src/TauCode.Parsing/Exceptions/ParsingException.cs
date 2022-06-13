using System;
using System.Runtime.Serialization;

namespace TauCode.Parsing.Exceptions
{
    [Serializable]
    public class ParsingException : Exception
    {
        public ParsingException()
        {
        }

        public ParsingException(string message)
            : base(message)
        {
        }

        public ParsingException(string message, int? index)
            : base(message)
        {
            this.Index = index;
        }

        public ParsingException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public ParsingException(string message, Exception inner, int? index)
            : base(message, inner)
        {
            this.Index = index;
        }

        protected ParsingException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
            info.AddValue(nameof(Index), this.Index);
        }

        public int? Index { get; }
    }
}
