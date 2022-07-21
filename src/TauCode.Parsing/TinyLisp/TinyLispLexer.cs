using System;
using TauCode.Parsing.TinyLisp.TokenProducers;
using TauCode.Parsing.TokenProducers;

namespace TauCode.Parsing.TinyLisp
{
    public sealed class TinyLispLexer : Lexer
    {
        public TinyLispLexer()
        {
            // todo: prohibit changing producers
            this.Producers = new ILexicalTokenProducer[]
            {
                new WhiteSpaceProducer(), // NB: it is very important that this one goes first. it will skip white spaces without producing a real token.
                new TinyLispPunctuationProducer(),
                new TinyLispStringProducer(),
                new Int32Producer(IntegerTerminatorPredicate),
                new Int64Producer(IntegerTerminatorPredicate),
                new TinyLispSymbolProducer(),
                new TinyLispKeywordProducer(),
                new TinyLispCommentProducer(),
            };
        }

        private static bool IntegerTerminatorPredicate(ReadOnlySpan<char> input, int position)
        {
            var c = input[position];

            if (c.IsInlineWhiteSpaceOrCaretControl())
            {
                return true;
            }

            if (TinyLispHelper.IsPunctuation(c))
            {
                return true;
            }

            return false;
        }
    }
}
