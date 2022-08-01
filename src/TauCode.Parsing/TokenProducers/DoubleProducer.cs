using TauCode.Data.Text;
using TauCode.Data.Text.TextDataExtractors;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TokenProducers;

public class DoubleProducer : LexicalTokenProducerBase
{
    private readonly DoubleExtractor _extractor;

    public DoubleProducer(TerminatingDelegate? terminator = null)
    {
        _extractor = new DoubleExtractor(terminator);
    }

    protected override ILexicalToken? ProduceImpl(LexingContext context)
    {
        var start = context.Position;

        var span = context.Input.Span[context.Position..];
        var extractionResult = _extractor.TryExtract(span, out var value);

        if (extractionResult.ErrorCode.HasValue)
        {
            return null;
        }

        var consumed = extractionResult.CharsConsumed;
        return new DoubleToken(start, consumed, value);
    }
}