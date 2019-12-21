﻿using System.Linq;
using TauCode.Extensions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.TokenExtractors
{
    public class SqlPunctuationExtractor : TokenExtractorBase
    {
        public SqlPunctuationExtractor()
            : base(StandardLexingEnvironment.Instance, SqlPunctuationFirstCharPredicate)
        {
        }

        private static bool SqlPunctuationFirstCharPredicate(char c)
        {
            return c.IsIn('(', ')', ',');
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();

            return new PunctuationToken(str.Single());
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();
            var pos = this.GetLocalPosition();

            if (pos == 0)
            {
                return CharChallengeResult.Continue;
            }

            return CharChallengeResult.Finish; // whatever it is - it's a single-char token extractor.
        }

        protected override CharChallengeResult ChallengeEnd() => CharChallengeResult.Finish;
    }
}
