namespace TauCode.Parsing
{
    internal enum ParsingErrorTag
    {
        // Lexing
        UnclosedString = 1000,
        NewLineInString,
        BadEscape,
        CannotTokenize,

        // Tiny Lisp
        TinyLispBadKeyword = 2000,
        TinyLispBadSymbolName,
        TinyLispUnclosedForm,
        TinyLispUnexpectedRightParenthesis,
        TinyLispCannotReadToken
    }
}
