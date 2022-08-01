using TauCode.Extensions;

namespace TauCode.Parsing.Tests;

internal static class Helper
{
    private static readonly HashSet<char> StandardPunctuationChars;

    static Helper()
    {
        var punctList = new List<char>();
        punctList.AddRange(new[]
        {
            '~',
            '?',
            '!',
            '@',
            '#',
            '$',
            '%',
            '^',
            '&',
            '=',
            '*',
            '|',
            '/',
            '+',
            '-',
            '[',
            ']',
            '(',
            ')',
            '{',
            '}',
            '\\',
            '.',
            ',',
            '"',
            '\'',
            ':',
            ';',
            '`',
        });
        StandardPunctuationChars = new HashSet<char>(punctList);


    }

    internal static bool IsLatinLetterInternal(this char c)
    {
        if (c >= 'a' && c <= 'z')
        {
            return true;
        }

        if (c >= 'A' && c <= 'Z')
        {
            return true;
        }

        return false;
    }

    internal static bool IsDecimalDigit(this char c)
    {
        return c >= '0' && c <= '9';
    }

    internal static bool IsInlineWhiteSpaceOrCaretControl(this char c) => IsInlineWhiteSpace(c) || IsCaretControl(c);

    internal static bool IsStandardPunctuationChar(this char c) => StandardPunctuationChars.Contains(c);

    internal static bool IsInlineWhiteSpace(this char c) => c.IsIn(' ', '\t');

    internal static bool IsCaretControl(this char c) => c.IsIn('\r', '\n');

    public static bool IsWhiteSpace(ReadOnlySpan<char> input, int pos)
    {
        var c = input[pos];
        var result = c.IsIn(' ', '\t', '\r', '\n');
        return result;
    }
}
