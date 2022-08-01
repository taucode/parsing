using TauCode.Data.Text;
using TauCode.Data.Text.TextDataExtractors;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TokenProducers;

public class EmailAddressProducer : LexicalTokenProducerBase
{
    private readonly EmailAddressExtractor _extractor;

    public EmailAddressProducer(TerminatingDelegate? terminator = null)
    {
        _extractor = new EmailAddressExtractor(terminator);
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
        return new EmailAddressToken(start, consumed, value!);
    }
}