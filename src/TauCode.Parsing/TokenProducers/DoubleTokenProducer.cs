using System;
using System.Collections.Generic;

namespace TauCode.Parsing.TokenProducers
{
    public class DoubleTokenProducer : ILexicalTokenProducer
    {
        private readonly HashSet<char> _terminatingChars;

        public DoubleTokenProducer(IEnumerable<char> terminatingChars)
        {
            // todo checks

            _terminatingChars = new HashSet<char>(terminatingChars);
        }

        public const int MaxLength = 100;

        public ILexicalToken Produce(LexingContext context)
        {
            var start = context.Position;
            var input = context.Input.Span[context.Position..];
            var pos = 0;

            //char? sign = null;
            char? prevChar = null;
            var gotExp = false; // got exponent (like in 1e+3)
            var gotPeriod = false;

            while (true)
            {
                var c = input[pos];

                if (c == '+' || c == '-')
                {
                    if (pos == 0 || prevChar == 'e' || prevChar == 'E')
                    {
                        // that's all right
                    }
                    else
                    {
                        // the only option is termination
                        throw new NotImplementedException(); // todo
                    }
                }
                else if (c.IsDecimalDigit())
                {
                    // that's all right
                }
                else if (c == '.')
                {
                    if (gotPeriod)
                    {
                        return null; // duplicate period, cannot be double
                    }

                    gotPeriod = true;

                    // that's all right
                }
                else if (c == 'e' || c == 'E')
                {
                    if (gotExp)
                    {
                        return null;
                    }

                    gotExp = true;
                }
                else if (_terminatingChars.Contains(c))
                {
                    break;
                }

                prevChar = c;
                pos++;
            }

            throw new NotImplementedException();
        }
    }
}
