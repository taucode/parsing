using TauCode.Data.Text;
using TauCode.Data.Text.TextDataExtractors;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TokenProducers;

public class CLangStringProducer : LexicalTokenProducerBase
{
    private readonly CLangStringExtractor _extractor;

    public CLangStringProducer(TerminatingDelegate? terminator = null)
    {
        _extractor = new CLangStringExtractor(terminator);
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

        return new StringToken(start, extractionResult.CharsConsumed, value!, "CLang");
    }
}