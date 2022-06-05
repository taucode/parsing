using System;
using System.Collections.Generic;
using System.Text;

namespace Tau.Kappa.Parsing
{
    internal enum ParsingErrorTag
    {
        // Lexing
        UnclosedString = 1000,
        NewLineInString,
        BadEscape,
        CannotTokenize
    }
}
