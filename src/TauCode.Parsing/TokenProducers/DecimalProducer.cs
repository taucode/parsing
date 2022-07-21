using TauCode.Data.Text;
using TauCode.Data.Text.TextDataExtractors;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TokenProducers
{
    public class DecimalProducer : LexicalTokenProducerBase
    {
        private readonly DecimalExtractor _extractor;

        public DecimalProducer(TerminatingDelegate terminator = null)
        {
            _extractor = new DecimalExtractor(terminator);
        }

        protected override ILexicalToken ProduceImpl(LexingContext context)
        {
            var start = context.Position;

            var span = context.Input.Span[context.Position..];
            var extractionResult = _extractor.TryExtract(span, out var value);
            if (extractionResult.ErrorCode.HasValue)
            {
                return null;
            }

            return new DecimalToken(start, extractionResult.CharsConsumed, value);
        }
    }
}
