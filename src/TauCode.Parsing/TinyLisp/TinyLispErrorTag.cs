using System;
using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.TinyLisp
{
    internal enum TinyLispErrorTag
    {
        BadKeyword = 1,
        BadSymbolName,
        UnclosedForm,
        UnexpectedRightParenthesis,
        CannotReadToken
    }
}
