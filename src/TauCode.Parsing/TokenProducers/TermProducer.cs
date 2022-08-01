using TauCode.Data.Text;
using TauCode.Data.Text.TextDataExtractors;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TokenProducers;

public class TermProducer : LexicalTokenProducerBase
{
    private readonly TermExtractor _extractor;

    public TermProducer(TerminatingDelegate? terminator = null)
    {
        _extractor = new TermExtractor(terminator);
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

        return new TermToken(start, extractionResult.CharsConsumed, value!);
    }
}