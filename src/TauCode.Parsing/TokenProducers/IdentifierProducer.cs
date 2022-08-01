using TauCode.Data.Text;
using TauCode.Data.Text.TextDataExtractors;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TokenProducers;

public class IdentifierProducer : LexicalTokenProducerBase
{
    private readonly IdentifierExtractor _extractor;

    public IdentifierProducer(
        Func<string, bool> reservedWordPredicate,
        TerminatingDelegate? terminator = null)
    {
        _extractor = new IdentifierExtractor(reservedWordPredicate, terminator);
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

        return new IdentifierToken(start, extractionResult.CharsConsumed, value!);
    }
}