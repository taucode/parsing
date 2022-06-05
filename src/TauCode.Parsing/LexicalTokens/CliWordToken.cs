namespace TauCode.Parsing.LexicalTokens
{
    public class CliWordToken : TextToken
    {
        public CliWordToken(int position, int consumedLength, string text)
            : base(position, consumedLength, text)
        {
        }
    }
}
