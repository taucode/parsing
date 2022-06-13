using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TokenProducers
{
    public class CliWordProducer : ILexicalTokenProducer
    {
        public ILexicalToken Produce(LexingContext context)
        {
            // todo checks. consider LexicalTokenProducerBase + ProduceImpl

            var start = context.Position;
            var input = context.Input.Span[start..];
            var pos = 0;

            char? prevChar = null;
            char? lastChar = null;

            while (true)
            {
                if (pos == input.Length)
                {
                    break;
                }

                var c = input[pos];
                lastChar = c;

                if (c.IsDecimalDigit())
                {
                    if (pos == 0)
                    {
                        return null;
                    }

                    throw new NotImplementedException();
                }
                else if (c.IsLatinLetterInternal())
                {
                    // ok, go on
                }
                else if (c == '-')
                {
                    if (pos == 0 || prevChar == '-')
                    {
                        return null;
                    }

                    // ok, go on
                }
                else if (c.IsInlineWhiteSpaceOrCaretControl())
                {
                    break;
                }
                else
                {
                    return null;
                }

                prevChar = c;

                pos++;
            }

            if (pos == 0 || lastChar == '-')
            {
                return null;
            }

            var text = input[..pos].ToString();
            context.Position += pos;

            var token = new CliWordToken(start, pos, text);

            return token;
        }
    }
}
