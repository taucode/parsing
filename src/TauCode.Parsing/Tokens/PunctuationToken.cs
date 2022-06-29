namespace TauCode.Parsing.Tokens
{
    public class PunctuationToken : ValueTokenBase<char>
    {
        public PunctuationToken(
            int position,
            int consumedLength,
            char value)
            : base(
                position,
                consumedLength,
                value,
                value.ToString())
        {
        }
    }
}
