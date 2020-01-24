﻿using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Sql.Producers
{
    public class WordProducer : ITokenProducer
    {
        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var text = context.Text;
            var length = text.Length;

            var c = text[context.Index];

            if (LexingHelper.IsLatinLetter(c) || c == '_')
            {
                var initialIndex = context.Index;
                var index = initialIndex + 1;
                var column = context.Column + 1;

                while (true)
                {
                    if (index == length)
                    {
                        break;
                    }

                    c = text[index];

                    if (
                        LexingHelper.IsInlineWhiteSpaceOrCaretControl(c) ||
                        LexingHelper.IsStandardPunctuationChar(c))
                    {
                        break;
                    }

                    if (c == '_' || LexingHelper.IsLatinLetter(c) || LexingHelper.IsDigit(c))
                    {
                        index++;
                        column++;

                        continue;
                    }

                    return null;
                }

                var delta = index - initialIndex;
                var str = text.Substring(initialIndex, delta);

                context.Advance(delta, 0, column);

                return new TextToken(
                    WordTextClass.Instance,
                    NoneTextDecoration.Instance,
                    str,
                    new Position(context.Line, column), delta);
            }

            return null;
        }
    }
}
