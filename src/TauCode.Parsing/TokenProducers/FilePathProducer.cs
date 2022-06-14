using TauCode.Extensions;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TokenProducers
{
    public class FilePathProducer : ILexicalTokenProducer
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

                if (c.IsInlineWhiteSpaceOrCaretControl())
                {
                    break;
                }
                else if (c.IsIn('?', '*'))
                {
                    return null;
                }
                else if (c == ':')
                {
                    if (pos == 1)
                    {
                        // ok
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    // ok
                }

                pos++;
            }

            if (pos == 0)
            {
                return null;
            }

            var path = input[..pos].ToString();
            context.Position += pos;

            var token = new FilePathToken(start, pos, path);
            return token;
        }
    }
}
