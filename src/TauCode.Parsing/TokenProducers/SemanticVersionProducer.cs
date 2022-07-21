using TauCode.Data.Text;
using TauCode.Data.Text.TextDataExtractors;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TokenProducers
{
    public class SemanticVersionProducer : LexicalTokenProducerBase
    {
        private readonly SemanticVersionExtractor _extractor;

        public SemanticVersionProducer(TerminatingDelegate terminator = null)
        {
            _extractor = new SemanticVersionExtractor(terminator);
        }

        protected override ILexicalToken ProduceImpl(LexingContext context)
        {
            var start = context.Position;
            var span = context.Input.Span[context.Position..];

            var result = _extractor.TryExtract(span, out var value);
            if (result.ErrorCode.HasValue)
            {
                return null;
            }

            return new SemanticVersionToken(start, result.CharsConsumed, value);
        }
    }
}
