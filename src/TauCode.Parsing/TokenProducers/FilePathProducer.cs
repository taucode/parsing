using TauCode.Data.Text;
using TauCode.Data.Text.TextDataExtractors;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TokenProducers
{
    public class FilePathProducer : LexicalTokenProducerBase
    {
        private readonly FilePathExtractor _extractor;

        public FilePathProducer(TerminatingDelegate terminator = null)
        {
            _extractor = new FilePathExtractor(terminator);
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

            return new FilePathToken(start, extractionResult.CharsConsumed, value);
        }
    }
}
