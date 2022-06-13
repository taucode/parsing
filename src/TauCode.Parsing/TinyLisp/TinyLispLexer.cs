using TauCode.Parsing.LexicalTokenProducers;
using TauCode.Parsing.TinyLisp.Producers;

namespace TauCode.Parsing.TinyLisp
{
    public class TinyLispLexer : Lexer
    {
        public TinyLispLexer()
        {
            this.Producers = new ILexicalTokenProducer[]
            {
                new WhiteSpaceProducer(), // NB: it is very important that this one goes first. it will skip white spaces without producing a real token.
                new TinyLispPunctuationProducer(),
                new TinyLispStringProducer(),
                new IntegerProducer(IntegerTerminatorPredicate),
                new TinyLispSymbolProducer(),
                new TinyLispKeywordProducer(),
                new TinyLispCommentProducer(),
            };
        }

        private static bool IntegerTerminatorPredicate(char c)
        {
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
