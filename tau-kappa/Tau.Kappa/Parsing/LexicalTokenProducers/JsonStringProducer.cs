using System;
using System.Collections.Generic;
using System.Text;
using Tau.Kappa.Parsing.LexicalTokens;

namespace Tau.Kappa.Parsing.LexicalTokenProducers
{
    public class JsonStringProducer : ILexicalTokenProducer
    {
        public ILexicalToken Produce(LexingContext context)
        {
            var start = context.Position;
            var input = context.Input.Span[start..];
            var pos = 0;

            var c = input[0];

            char closingChar;

            if (c == '"' || c == '\'')
            {
                closingChar = c;
            }
            else
            {
                return null;
            }

            pos++; // skip opening delimiter

            var sb = new StringBuilder();

            while (true)
            {
                if (pos == input.Length)
                {
                    throw Helper.CreateException(ParsingErrorTag.UnclosedString, pos);
                }

                c = input[pos];

                if (c == closingChar)
                {
                    pos++;
                    break;
                }
                else if (c == '\\')
                {
                    throw new NotImplementedException();
                }
                else
                {
                    // go on
                }

                sb.Append(c);
                pos++;
            }

            context.Position += pos; // skip closing char

            var token = new StringToken(start, pos, sb.ToString(), "Json");
            return token;
        }
    }
}
