using TauCode.Data.Text;
using TauCode.Data.Text.TextDataExtractors;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TokenProducers;

public class WordProducer : LexicalTokenProducerBase
{
    private readonly WordExtractor _extractor;

    public WordProducer(TerminatingDelegate? terminator = null)
    {
        _extractor = new WordExtractor(terminator);
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

        return new WordToken(start, extractionResult.CharsConsumed, value!);
    }
}