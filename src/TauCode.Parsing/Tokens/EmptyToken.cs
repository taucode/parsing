namespace TauCode.Parsing.Tokens;

public sealed class EmptyToken : LexicalTokenBase, IEmptyLexicalToken
{
    public EmptyToken(int position, int consumedLength)
        : base(position, consumedLength)
    {
    }
}