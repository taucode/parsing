using System;
using TauCode.Parsing.LexicalTokens;

namespace TauCode.Parsing.LexicalTokenProducers
{
    public class CliKeyProducer : ILexicalTokenProducer
    {
        public ILexicalToken Produce(LexingContext context)
        {
            var start = context.Position;
            var input = context.Input.Span[start..];
            var pos = 0;

            char? lastChar = null;
            var hyphensInRow = 0;

            while (true)
            {
                if (pos == input.Length)
                {
                    break;
                }

                var c = input[pos];

                if (pos == 0 && c != '-')
                {
                    return null;
                }

                if (c == '-')
                {
                    hyphensInRow++;
                    if (hyphensInRow == 2)
                    {
                        if (pos == 1)
                        {
                            // ok
                        }
                        else
                        {
                            // todo: ut all 'return null;' in all producers
                            return null; // '---some-key' is invalid
                        }
                    }

                    if (hyphensInRow > 2)
                    {
                        return null;
                    }
                }
                else if (c.IsDecimalDigit())
                {
                    hyphensInRow = 0;

                    if (pos == 0)
                    {
                        return null;
                    }

                    throw new NotImplementedException();
                }
                else if (c.IsLatinLetterInternal())
                {
                    hyphensInRow = 0;

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

                lastChar = c;

                pos++;
            }

            if (pos == 0)
            {
                return null;
            }

            if (lastChar == '-')
            {
                return null;
            }

            var token = new CliKeyToken(start, pos, input[..pos].ToString());
            context.Position += pos;
            return token;
        }
    }
}
