using System;

namespace TauCode.Parsing.Exceptions
{
    [Serializable]
    public class GraphException : ParsingExceptionBase
    {
        public GraphException()
        {
        }

        public GraphException(string message)
            : base(message)
        {
        }

        public GraphException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
