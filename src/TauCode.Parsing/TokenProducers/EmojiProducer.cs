using TauCode.Data.Text.TextDataExtractors;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TokenProducers;

public class EmojiProducer : LexicalTokenProducerBase
{
    protected override ILexicalToken? ProduceImpl(LexingContext context)
    {
        var start = context.Position;
        var span = context.Input.Span[context.Position..];

        var extractionResult = EmojiExtractor.Instance.TryExtract(span, out var value);
        if (extractionResult.ErrorCode.HasValue)
        {
            return null;
        }

        return new EmojiToken(start, extractionResult.CharsConsumed, value);
    }
}