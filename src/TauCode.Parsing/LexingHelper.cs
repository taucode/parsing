using System;
using TauCode.Extensions;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing
{
    internal static class LexingHelper
    {
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

        internal static bool IsCaretControl(this char c) => c.IsIn('\r', '\n');

        internal static bool IsInlineWhiteSpace(this char c) => c.IsIn(' ', '\t');

        internal static bool IsInlineWhiteSpaceOrCaretControl(this char c) => IsInlineWhiteSpace(c) || IsCaretControl(c);

        internal static LexingException CreateException(LexingErrorTag errorTag, int? index, params object[] formattingParams)
        {
            var message = GetErrorMessage(errorTag);

            if (formattingParams.Length > 0)
            {
                message = string.Format(message, formattingParams);
            }

            if (index.HasValue)
            {
                message += $"{Environment.NewLine}Index in text: {index.Value}.";
            }

            var ex = new LexingException(message, index);
            return ex;
        }

        private static string GetErrorMessage(LexingErrorTag errorTag)
        {
            return errorTag switch
            {
                // Lexing
                LexingErrorTag.UnclosedString => "Unclosed string.",
                LexingErrorTag.NewLineInString => "Newline in string.",
                LexingErrorTag.BadEscape => "Bad escape sequence.",
                LexingErrorTag.CannotTokenize => "Cannot tokenize.",

                _ => "Unknown error"
            };
        }
    }
}
