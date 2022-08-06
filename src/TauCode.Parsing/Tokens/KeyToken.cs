namespace TauCode.Parsing.Tokens;

public class KeyToken : TextTokenBase
{
    public KeyToken(
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