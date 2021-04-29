using System;
using System.Collections.Generic;
using System.Text;
using Tau.Kappa.Parsing.Exceptions;
using TauCode.Extensions;

namespace Tau.Kappa.Parsing
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

        internal static ParsingException CreateException(ParsingErrorTag errorTag, int? index)
        {
            var message = Helper.GetErrorMessage(errorTag);
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
                ParsingErrorTag.UnclosedString => "Unclosed string.",
                ParsingErrorTag.NewLineInString => "Newline in string.",
                ParsingErrorTag.BadEscape => "Bad escape sequence.",
                ParsingErrorTag.CannotTokenize => "Cannot tokenize.",
                _ => "Unknown error"
            };
        }
    }
}
