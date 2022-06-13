namespace TauCode.Parsing.Tokens
{
    public class IdentifierToken : TextToken
    {
        public IdentifierToken(int position, int consumedLength, string text)
            : base(position, consumedLength, text)
        {
        }
    }
}
