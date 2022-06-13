using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Sql.Producers;

public class WordProducer : ILexicalTokenProducer
{
    public ILexicalToken Produce(LexingContext context)
    {
        var text = context.Input.Span;
        var length = text.Length;

        var c = text[context.Position];

        if (c.IsLatinLetterInternal() || c == '_')
        {
            var initialIndex = context.Position;
            var index = initialIndex + 1;

            while (true)
            {
                if (index == length)
                {
                    break;
                }

                c = text[index];

                if (
                    c.IsInlineWhiteSpaceOrCaretControl() ||
                    c.IsStandardPunctuationChar())
                {
                    break;
                }

                if (c == '_' || c.IsLatinLetterInternal() || c.IsDecimalDigit())
                {
                    index++;

                    continue;
                }

                return null;
            }

            var delta = index - initialIndex;
            var str = text.Slice(initialIndex, delta).ToString();

            context.Position += delta;

            return new WordToken(
                initialIndex,
                delta,
                str);
        }

        return null;
    }
}