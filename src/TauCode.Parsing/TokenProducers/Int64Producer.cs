using TauCode.Data.Text;
using TauCode.Data.Text.TextDataExtractors;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TokenProducers;

public class Int64Producer : LexicalTokenProducerBase
{
    private readonly Int64Extractor _extractor;

    public Int64Producer(TerminatingDelegate? terminator = null)
    {
        _extractor = new Int64Extractor(terminator);
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

        return new Int64Token(start, extractionResult.CharsConsumed, value);
    }
}