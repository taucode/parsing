using System.Globalization;

namespace TauCode.Parsing.LexicalTokens
{
    public class IntegerToken : TextToken
    {
        public IntegerToken(
            int position,
            int consumedLength,
            long value) :
            base(
                position,
                consumedLength,
                value.ToString(CultureInfo.InvariantCulture))
        {
            this.Value = value;
        }

        public long Value { get; }
    }
}
