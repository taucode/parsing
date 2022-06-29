using System;

namespace TauCode.Parsing.Exceptions
{
    [Serializable]
    public class ReadingException : ParsingExceptionBase
    {
        public ReadingException()
        {
        }

        public ReadingException(string message)
            : base(message)
        {
        }

        public ReadingException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
