using System;
using TauCode.Extensions;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing
{
    internal static class Helper
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

        internal static ParsingException CreateException(ParsingErrorTag errorTag, int? index, params object[] formattingParams)
        {
            var message = Helper.GetErrorMessage(errorTag);

            if (formattingParams.Length > 0)
            {
                message = string.Format(message, formattingParams);
            }

            if (index.HasValue)
            {
                message += $"{Environment.NewLine}Index in text: {index.Value}.";
            }

            var ex = new ParsingException(message, index);
            return ex;
        }

        private static string GetErrorMessage(ParsingErrorTag errorTag)
        {
            return errorTag switch
            {
                // Lexing
                ParsingErrorTag.UnclosedString => "Unclosed string.",
                ParsingErrorTag.NewLineInString => "Newline in string.",
                ParsingErrorTag.BadEscape => "Bad escape sequence.",
                ParsingErrorTag.CannotTokenize => "Cannot tokenize.",

                // Tiny Lisp
                ParsingErrorTag.TinyLispBadKeyword => "todo",
                ParsingErrorTag.TinyLispBadSymbolName => "Bad symbol name.",
                ParsingErrorTag.TinyLispUnclosedForm => "todo",
                ParsingErrorTag.TinyLispUnexpectedRightParenthesis => "todo",
                ParsingErrorTag.TinyLispCannotReadToken => "todo",

                _ => "Unknown error"
            };
        }
    }
}
