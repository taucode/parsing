namespace TauCode.Parsing.Tokens
{
    public class PunctuationToken : LexicalTokenBase
    {
        public PunctuationToken(int position, int consumedLength, char value)
            : base(position, consumedLength)
        {
            // todo checks
            this.Value = value;
        }

        public char Value { get; }

        public override string ToString() => this.Value.ToString();
    }
}
