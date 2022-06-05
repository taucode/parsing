using TauCode.Parsing.LexicalTokens;

namespace TauCode.Parsing.TinyLisp.Producers
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
                        throw Helper.CreateException(ParsingErrorTag.UnclosedString, index);
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
                                throw Helper.CreateException(ParsingErrorTag.UnclosedString, index);
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

                context.Position += delta;
                return token;
            }

            return null;
        }
    }
}
