﻿using TauCode.Parsing.Old.Lexing;
using TauCode.Parsing.Old.Lexing.StandardTokenExtractors;
using TauCode.Parsing.Old.Tests.Parsing.Sql.TokenExtractors;

namespace TauCode.Parsing.Old.Tests.Parsing.Sql
{
    public class OldSqlLexer : OldLexerBase
    {
        protected override void InitTokenExtractors()
        {
            // word
            var wordExtractor = new OldWordExtractor();
            this.AddTokenExtractor(wordExtractor);

            // punctuation
            var punctuationExtractor = new OldSqlPunctuationExtractor();
            this.AddTokenExtractor(punctuationExtractor);

            // integer
            var integerExtractor = new OldIntegerExtractor();
            this.AddTokenExtractor(integerExtractor);

            // identifier
            var identifierExtractor = new OldSqlIdentifierExtractor();
            this.AddTokenExtractor(identifierExtractor);

            // *** Links ***
            wordExtractor.AddSuccessors(
                punctuationExtractor);

            punctuationExtractor.AddSuccessors(
                punctuationExtractor,
                wordExtractor,
                integerExtractor,
                identifierExtractor);

            integerExtractor.AddSuccessors(
                punctuationExtractor);

            identifierExtractor.AddSuccessors(
                punctuationExtractor);
        }
    }
}