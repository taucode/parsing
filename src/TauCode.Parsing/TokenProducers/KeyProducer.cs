using TauCode.Data.Text;
using TauCode.Data.Text.TextDataExtractors;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TokenProducers;

public class KeyProducer : LexicalTokenProducerBase
{
    private readonly KeyExtractor _extractor;

    public KeyProducer(TerminatingDelegate? terminator = null)
    {
        _extractor = new KeyExtractor(terminator);
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

        return new KeyToken(start, extractionResult.CharsConsumed, value!);
    }
}