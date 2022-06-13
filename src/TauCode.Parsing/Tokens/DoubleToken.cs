using System.Globalization;

namespace TauCode.Parsing.Tokens
{
    public class DoubleToken : TextToken
    {
        public DoubleToken(
            int position,
            int consumedLength,
            double value)
            : base(
                position,
                consumedLength,
                value.ToString(CultureInfo.InvariantCulture))
        {
            this.Value = value;
        }

        public double Value { get; }
    }
}
