namespace TauCode.Parsing
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
