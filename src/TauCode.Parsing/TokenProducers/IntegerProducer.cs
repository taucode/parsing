using System;
using System.Globalization;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TokenProducers
{
    public class IntegerProducer : ILexicalTokenProducer
    {
        public IntegerProducer(Func<char, bool> terminatingPredicate = null)
        {
            this.TerminatingPredicate = terminatingPredicate ?? DefaultTerminatingPredicate;
        }

        public Func<char, bool> TerminatingPredicate { get; }

        private static bool DefaultTerminatingPredicate(char c)
        {
            return c.IsInlineWhiteSpaceOrCaretControl();
        }


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

                if (c == '-' || c == '+')
                {
                    if (pos == 0)
                    {
                        // ok
                    }
                    else
                    {
                        if (this.TerminatingPredicate(c))
                        {
                            // '+' and '-' can be terminating char, e.g. in c#: '-12+10' means '-12 + 10'
                            break;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                else if (c.IsDecimalDigit())
                {
                    // ok
                }
                else if (this.TerminatingPredicate(c))
                {
                    // terminate
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
