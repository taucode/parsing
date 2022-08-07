namespace TauCode.Parsing.Tokens;

public class IdentifierToken : TextTokenBase
{
    public IdentifierToken(
        int position,
        int consumedLength,
        string text)
        : base(
            position,
            consumedLength,
            text)
    {
    }
}