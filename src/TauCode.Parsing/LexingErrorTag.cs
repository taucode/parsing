namespace TauCode.Parsing;

internal enum LexingErrorTag
{
    UnclosedString = 1,
    NewLineInString,
    BadEscape,
    CannotTokenize,
}