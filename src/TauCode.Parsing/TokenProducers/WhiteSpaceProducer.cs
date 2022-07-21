using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TokenProducers
{
    public class WhiteSpaceProducer : LexicalTokenProducerBase, IEmptyLexicalTokenProducer
    {
        protected override ILexicalToken ProduceImpl(LexingContext context)
        {
            var skipped = this.Skip(context);
            if (skipped > 0)
            {
                return new EmptyToken(context.Position, skipped);
            }

            return null;
        }

        public int Skip(LexingContext context)
        {
            var start = context.Position;
            var input = context.Input.Span;
            var length = input.Length;

            var c = input[start];
            if (!c.IsInlineWhiteSpaceOrCaretControl())
            {
                return 0;
            }

            var pos = start;
            var goOn = true;

            while (goOn)
            {
                if (pos == length)
                {
                    break;
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
                        goOn = false;
                        break;
                }
            }

            var delta = pos - start;
            return delta;
        }
    }
}
