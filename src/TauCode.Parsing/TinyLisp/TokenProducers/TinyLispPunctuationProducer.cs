using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenProducers
{
    public class TinyLispPunctuationProducer : ILexicalTokenProducer
    {
        public ILexicalToken Produce(LexingContext context)
        {
            var text = context.Input.Span;
            var start = context.Position;

            var c = text[context.Position];
            var punctuation = TinyLispHelper.TryCharToPunctuation(c);

            if (punctuation.HasValue)
            {
                context.Position++;
                return new LispPunctuationToken(
                    start,
                    1,
                    punctuation.Value);
            }

            return null;
        }
    }
}
