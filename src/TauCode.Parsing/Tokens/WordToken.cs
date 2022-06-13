namespace TauCode.Parsing.Tokens
{
    public class WordToken : TextToken
    {
        public WordToken(int position, int consumedLength, string text)
            : base(position, consumedLength, text)
        {
        }
    }
}
