namespace TauCode.Parsing.Tokens
{
    public class StringToken : TextTokenBase
    {
        public StringToken(
            int position,
            int consumedLength,
            string text,
            string kind)
            : base(
                position,
                consumedLength,
                text)
        {
            this.Kind = kind;
        }

        public string Kind { get; }
    }
}
