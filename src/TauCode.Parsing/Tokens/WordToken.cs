namespace TauCode.Parsing.Tokens
{
    public class WordToken : TextTokenBase
    {
        public WordToken(
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
}
