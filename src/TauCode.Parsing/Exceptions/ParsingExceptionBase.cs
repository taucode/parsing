using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TauCode.Parsing.Exceptions
{
    public abstract class ParsingExceptionBase : Exception
    {
        protected ParsingExceptionBase()
        {
        }

        protected ParsingExceptionBase(string message) : base(message)
        {
        }

        protected ParsingExceptionBase(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
