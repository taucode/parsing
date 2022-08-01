using TauCode.Parsing.Exceptions;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenProducers
{
    public class TinyLispSymbolProducer : ILexicalTokenProducer
    {
        public ILexicalToken? Produce(LexingContext context)
        {
            var text = context.Input.Span;
            var length = text.Length;

            var c = text[context.Position];

            if (TinyLispHelper.IsAcceptableSymbolNameChar(c))
            {
                var gotSign = c == '+' || c == '-';
                var pureDigits = 0;
                if (!gotSign)
                {
                    pureDigits = c.IsDecimalDigit() ? 1 : 0;
                }

                var gotNonDigits = false;

                var initialIndex = context.Position;
                var index = initialIndex + 1;

                while (true)
                {
                    if (index == length)
                    {
                        break;
                    }

                    c = text[index];

                    if (c == ':')
                    {
                        throw TinyLispHelper.CreateException(TinyLispErrorTag.BadSymbolName, index);
                    }

                    if (!TinyLispHelper.IsAcceptableSymbolNameChar(c))
                    {
                        break;
                    }

                    if (c.IsDecimalDigit())
                    {
                        if (!gotNonDigits)
                        {
                            pureDigits++;
                        }
                    }
                    else
                    {
                        gotNonDigits = true;
                        pureDigits = 0;
                    }

                    index++;
                }

                var couldBeInt = pureDigits > 0;

                if (couldBeInt)
                {
                    throw new TinyLispException("Symbol producer delivered an integer.", initialIndex);
                }

                var delta = index - initialIndex;
                var str = text.Slice(initialIndex, delta);
                var symbolToken = new LispSymbolToken(initialIndex, delta, str.ToString());
                return symbolToken;
            }
            else
            {
                return null;
            }
        }
    }
}
