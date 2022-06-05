using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TauCode.Parsing.LexicalTokens;

namespace TauCode.Parsing.LexicalTokenProducers
{
    public class CLangStringProducer : ILexicalTokenProducer
    {
        private static readonly string[] ReplacementStrings =
        {
            "\"\"",
            "\\\\",
            "0\0",
            "a\a",
            "b\b",
            "f\f",
            "n\n",
            "r\r",
            "t\t",
            "v\v",
        };

        private static readonly Dictionary<char, char> Replacements;

        private static char? GetReplacement(char escape)
        {
            if (Replacements.TryGetValue(escape, out var replacement))
            {
                return replacement;
            }

            return null;
        }

        static CLangStringProducer()
        {
            Replacements = ReplacementStrings
                .ToDictionary(
                    x => x.First(),
                    x => x.Skip(1).Single());
        }

        public ILexicalToken Produce(LexingContext context)
        {
            var text = context.Input.Span;
            var length = text.Length;

            var c = text[context.Position];

            if (c == '"')
            {
                var start = context.Position;
                var pos = start + 1; // skip '"'

                var sb = new StringBuilder();

                while (true)
                {
                    if (pos == length)
                    {
                        throw Helper.CreateException(ParsingErrorTag.UnclosedString, pos);
                    }

                    c = text[pos];

                    if (c.IsCaretControl())
                    {
                        throw Helper.CreateException(ParsingErrorTag.NewLineInString, pos);
                    }

                    if (c == '\\')
                    {
                        if (pos + 1 == length)
                        {
                            throw Helper.CreateException(ParsingErrorTag.UnclosedString, length);
                        }

                        var nextChar = text[pos + 1];
                        if (nextChar == 'u')
                        {
                            var remaining = length - (pos + 1);
                            if (remaining < 5)
                            {
                                throw Helper.CreateException(ParsingErrorTag.BadEscape, pos);
                            }

                            var hexNumString = text.Slice(pos + 2, 4);
                            var codeParsed = int.TryParse(
                                hexNumString,
                                NumberStyles.HexNumber,
                                CultureInfo.InvariantCulture,
                                out var code);

                            if (!codeParsed)
                            {
                                throw Helper.CreateException(ParsingErrorTag.BadEscape, pos);
                            }

                            var unescapedChar = (char)code;
                            sb.Append(unescapedChar);

                            pos += 6;
                            continue;
                        }
                        else
                        {
                            var replacement = GetReplacement(nextChar);
                            if (replacement.HasValue)
                            {
                                sb.Append(replacement);
                                pos += 2;
                                continue;
                            }
                            else
                            {
                                throw Helper.CreateException(ParsingErrorTag.BadEscape, pos);
                            }
                        }
                    }

                    pos++;

                    if (c == '"')
                    {
                        break;
                    }

                    sb.Append(c);
                }

                var delta = pos - start;
                var str = sb.ToString();

                var token = new StringToken(
                    start,
                    delta,
                    str,
                    "C");

                context.Position += delta;
                return token;
            }

            return null;
        }

        // todo clean
        //private void ThrowBadEscapeException(int line, int column)
        //{
        //    throw new LexingException("Bad escape.", new Position(line, column));
        //}
    }
}
