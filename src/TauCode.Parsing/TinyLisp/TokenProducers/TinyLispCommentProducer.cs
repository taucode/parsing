using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenProducers
{
    public class TinyLispCommentProducer : ILexicalTokenProducer
    {
        public ILexicalToken Produce(LexingContext context)
        {
            var text = context.Input.Span;
            var length = text.Length;

            var c = text[context.Position];

            if (c == ';')
            {
                var initialIndex = context.Position;
                var index = initialIndex + 1; // skip ';'

                while (true)
                {
                    if (index == length)
                    {
                        context.Position += index - initialIndex;
                        return EmptyToken.Instance;
                    }

                    c = text[index];
                    if (c.IsCaretControl())
                    {
                        context.Position += index - initialIndex;
                        return EmptyToken.Instance;
                    }

                    index++;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
