using TauCode.Data.Text;
using TauCode.Data.Text.TextDataExtractors;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TokenProducers
{
    public class Int32Producer : LexicalTokenProducerBase
    {
        private readonly Int32Extractor _extractor;

        public Int32Producer(TerminatingDelegate terminator = null)
        {
            _extractor = new Int32Extractor(terminator);
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

            var consumed = extractionResult.CharsConsumed;
            return new Int32Token(start, consumed, value);
        }
    }
}
