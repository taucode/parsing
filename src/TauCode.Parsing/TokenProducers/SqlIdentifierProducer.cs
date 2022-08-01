using TauCode.Data.Text;
using TauCode.Data.Text.TextDataExtractors;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TokenProducers;

public class SqlIdentifierProducer : LexicalTokenProducerBase
{
    private readonly SqlIdentifierExtractor _extractor;

    public SqlIdentifierProducer(Func<string, bool> reservedWordPredicate, TerminatingDelegate? terminator = null)
    {
        _extractor = new SqlIdentifierExtractor(reservedWordPredicate, terminator);
    }

    public SqlIdentifierDelimiter Delimiter
    {
        get => _extractor.Delimiter;
        set => _extractor.Delimiter = value;
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

        return new SqlIdentifierToken(start, extractionResult.CharsConsumed, value);
    }
}