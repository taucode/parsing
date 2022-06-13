namespace TauCode.Parsing.LexicalTokens
{
    public class CliKeyToken : TextToken
    {
        public CliKeyToken(int position, int consumedLength, string text)
            : base(position, consumedLength, text)
        {
        }
    }
}
