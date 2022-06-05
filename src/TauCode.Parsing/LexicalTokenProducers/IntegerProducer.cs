using System.Globalization;
using TauCode.Parsing.LexicalTokens;

namespace TauCode.Parsing.LexicalTokenProducers
{
    public class IntegerProducer : ILexicalTokenProducer
    {
        public ILexicalToken Produce(LexingContext context)
        {
            var start = context.Position;
            var input = context.Input.Span[start..];
            var pos = 0;

            while (true)
            {
                if (pos == input.Length)
                {
                    break;
                }

                var c = input[pos];

                if (c.IsDecimalDigit())
                {
                    // ok
                }
                else if (c.IsInlineWhiteSpaceOrCaretControl())
                {
                    break;
                }
                else
                {
                    return null;
                }

                pos++;
            }

            if (pos == 0)
            {
                return null;
            }

            var textSpan = input[..pos];

            var parsed = int.TryParse(textSpan, NumberStyles.Number, CultureInfo.InvariantCulture, out var value);
            if (parsed)
            {
                var token = new IntegerToken(start, pos, value);
                context.Position += pos;
                return token;
            }

            return null;
        }
    }
}
