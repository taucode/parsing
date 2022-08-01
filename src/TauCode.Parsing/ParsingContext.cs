namespace TauCode.Parsing;

public class ParsingContext
{
    public ParsingContext(IList<ILexicalToken> tokens, IParsingResult parsingResult)
    {
        // todo checks

        this.Tokens = tokens;
        this.ParsingResult = parsingResult;
    }

    public readonly IList<ILexicalToken> Tokens;
    public int Position;
    public IParsingResult ParsingResult { get; }
}