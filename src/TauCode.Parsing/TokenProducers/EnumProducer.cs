using TauCode.Data.Text;
using TauCode.Data.Text.TextDataExtractors;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TokenProducers
{
    public class EnumProducer<TEnum> : LexicalTokenProducerBase where TEnum : struct
    {
        private readonly EnumExtractor<TEnum> _extractor;

        public EnumProducer(
            bool ignoreCase = true,
            TerminatingDelegate terminator = null)
        {
            _extractor = new EnumExtractor<TEnum>(ignoreCase, terminator);
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
            return new EnumToken<TEnum>(start, consumed, value);
        }
    }
}
