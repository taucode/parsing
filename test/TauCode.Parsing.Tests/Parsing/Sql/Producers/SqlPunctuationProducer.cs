﻿using TauCode.Extensions;
using TauCode.Parsing.LexicalTokens;

namespace TauCode.Parsing.Tests.Parsing.Sql.Producers
{
    public class SqlPunctuationProducer : ILexicalTokenProducer
    {
        public ILexicalToken Produce(LexingContext context)
        {
            var text = context.Input.Span;

            var c = text[context.Position]; // todo: can throw

            if (c.IsIn('(', ')', ','))
            {
                var index = context.Position;
                context.Position++;
                return new PunctuationToken(index, 1, text[index]);
            }

            return null;
        }
    }
}
