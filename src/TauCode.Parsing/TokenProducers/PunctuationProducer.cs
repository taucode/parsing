using TauCode.Data.Text;
using TauCode.Data.Text.TextDataExtractors;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TokenProducers;

public class PunctuationProducer : LexicalTokenProducerBase
{
    private readonly PunctuationExtractor _extractor;

    public PunctuationProducer(
        IEnumerable<char> punctuationChars,
        TerminatingDelegate terminator)
    {
        _extractor = new PunctuationExtractor(punctuationChars, terminator);
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

        return new PunctuationToken(start, extractionResult.CharsConsumed, value);
    }
}