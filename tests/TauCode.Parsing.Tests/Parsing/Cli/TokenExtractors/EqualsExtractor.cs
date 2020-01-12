﻿using System;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.TokenExtractors
{
    public class EqualsExtractor : TokenExtractorBase<PunctuationToken>
    {
        public override PunctuationToken ProduceToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            return new PunctuationToken('=', position, consumedLength);
        }

        protected override void OnBeforeProcess()
        {
            // idle
        }

        protected override bool AcceptsPreviousTokenImpl(IToken previousToken)
        {
            throw new NotImplementedException();
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                return this.ContinueOrFail(c == '=');
            }

            return CharAcceptanceResult.Stop;
        }
    }
}
