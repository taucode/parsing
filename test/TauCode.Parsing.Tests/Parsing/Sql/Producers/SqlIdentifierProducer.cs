using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.LexicalTokens;

namespace TauCode.Parsing.Tests.Parsing.Sql.Producers
{
    public class SqlIdentifierProducer : ILexicalTokenProducer
    {
        private static Dictionary<char, char> Delimiters { get; }
        private static HashSet<char> OpeningDelimiters { get; }
        private static HashSet<char> ClosingDelimiters { get; }
        private static Dictionary<char, char> ReverseDelimiters { get; }

        static SqlIdentifierProducer()
        {
            Delimiters = new[]
                {
                    "[]",
                    "\"\"",
                    "``",
                }
                .ToDictionary(x => x[0], x => x[1]);

            OpeningDelimiters = new HashSet<char>(Delimiters.Keys);
            ClosingDelimiters = new HashSet<char>(Delimiters.Values);

            ReverseDelimiters = Delimiters
                .ToDictionary(x => x.Value, x => x.Key);

        }

        public ILexicalToken Produce(LexingContext context)
        {
            var text = context.Input.Span;
            var length = text.Length;

            var c = text[context.Position];

            if (OpeningDelimiters.Contains(c) ||
                c == '_' ||
                c.IsLatinLetterInternal())
            {
                var openingDelimiter = OpeningDelimiters.Contains(c) ? c : (char?)null;

                var initialIndex = context.Position;
                var index = initialIndex + 1;
                
                while (true)
                {
                    if (index == length)
                    {
                        if (openingDelimiter.HasValue)
                        {
                            this.ThrowUnclosedIdentifierException(context.Position);
                        }
                        break;
                    }

                    c = text[index];

                    if (c == '_' || c.IsLatinLetterInternal() || c.IsDecimalDigit())
                    {
                        index++;
                        continue;
                    }

                    if (ClosingDelimiters.Contains(c))
                    {
                        if (index - initialIndex > 2)
                        {
                            if (openingDelimiter.HasValue)
                            {
                                if (openingDelimiter.Value == ReverseDelimiters[c])
                                {
                                    index++;

                                    var delta = index - initialIndex;
                                    var str = text.Slice(initialIndex + 1, delta - 2).ToString();

                                    context.Position += delta;
                                    return new IdentifierToken(
                                        index,
                                        delta,
                                        str);
                                }
                                else
                                {
                                    this.ThrowUnclosedIdentifierException(index);
                                }
                            }
                            else
                            {
                                throw new ParsingException($"Unexpected delimiter: '{c}'.", index);
                            }
                        }
                        else
                        {
                            throw new ParsingException($"Unexpected delimiter: '{c}'.", index);
                        }
                    }
                }
            }

            return null;
        }

        private void ThrowUnclosedIdentifierException(int position)
        {
            throw new ParsingException("Unclosed identifier.", position);
        }
    }
}
