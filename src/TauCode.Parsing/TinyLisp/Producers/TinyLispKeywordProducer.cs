using TauCode.Parsing.Exceptions;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp.Producers
{
    public class TinyLispKeywordProducer : ILexicalTokenProducer
    {
        public ILexicalToken Produce(LexingContext context)
        {
            var text = context.Input.Span;
            var length = text.Length;

            var c = text[context.Position];

            if (c == ':')
            {
                var nameCharsCount = 0;
                var initialIndex = context.Position;
                var index = initialIndex + 1;

                while (true)
                {
                    if (index == length)
                    {
                        break;
                    }

                    c = text[index];

                    if (c == ':')
                    {
                        ThrowBadKeywordException(initialIndex);
                    }

                    if (!TinyLispHelper.IsAcceptableSymbolNameChar(c))
                    {
                        break;
                    }

                    nameCharsCount++;
                    index++;
                }

                if (nameCharsCount == 0)
                {
                    ThrowBadKeywordException(initialIndex);
                }

                var delta = index - initialIndex;
                var keywordNameSpan = text.Slice(initialIndex, delta);
                var token = new KeywordToken(
                    initialIndex,
                    delta,
                    keywordNameSpan.ToString());

                context.Position += delta;
                return token;
            }
            else
            {
                return null;
            }
        }

        private static void ThrowBadKeywordException(int? index)
        {
            throw Helper.CreateException(ParsingErrorTag.TinyLispBadKeyword, index);
        }
    }
}
