using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenProducers
{
    public class TinyLispStringProducer : ILexicalTokenProducer
    {
        public ILexicalToken Produce(LexingContext context)
        {
            var text = context.Input.Span;
            var length = text.Length;

            var c = text[context.Position];

            if (c == '"')
            {
                var initialIndex = context.Position;

                var index = initialIndex + 1; // skip '"'

                while (true)
                {
                    if (index == length)
                    {
                        throw LexingHelper.CreateException(LexingErrorTag.UnclosedString, index);
                    }

                    c = text[index];

                    switch (c)
                    {
                        case '\r':
                            index++;

                            if (index < length)
                            {
                                var nextChar = text[index];
                                if (nextChar == '\n')
                                {
                                    index++;
                                }
                            }
                            else
                            {
                                throw LexingHelper.CreateException(LexingErrorTag.UnclosedString, index);
                            }

                            continue;

                        case '\n':
                            index++;
                            continue;
                    }

                    index++;

                    if (c == '"')
                    {
                        break;
                    }
                }

                var delta = index - initialIndex;
                var str = text.Slice(initialIndex + 1, delta - 2);

                var token = new StringToken(
                    initialIndex,
                    delta,
                    str.ToString(),
                    "TinyLisp");

                return token;
            }

            return null;
        }
    }
}
