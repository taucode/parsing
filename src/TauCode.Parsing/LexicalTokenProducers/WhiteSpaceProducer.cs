using TauCode.Parsing.LexicalTokens;

namespace TauCode.Parsing.LexicalTokenProducers
{
    public class WhiteSpaceProducer : ILexicalTokenProducer
    {
        public ILexicalToken Produce(LexingContext context)
        {
            var start = context.Position;
            var input = context.Input.Span;
            var length = input.Length;

            var c = input[start];
            if (!c.IsInlineWhiteSpaceOrCaretControl())
            {
                return null;
            }

            var pos = start;

            while (true)
            {
                if (pos == length)
                {
                    context.Position += pos - start;
                    return EmptyToken.Instance;
                }

                c = input[pos];
                switch (c)
                {
                    case '\t':
                    case ' ':
                    case '\v':
                    case '\f':
                    case '\r':
                    case '\n':
                        pos++;
                        break;

                    default:
                        var delta = pos - start;
                        context.Position += delta;
                        return EmptyToken.Instance;
                }
            }
        }
    }
}
