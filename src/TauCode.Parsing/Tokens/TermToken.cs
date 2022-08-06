namespace TauCode.Parsing.Tokens;

public class TermToken : TextTokenBase
{
    public TermToken(
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