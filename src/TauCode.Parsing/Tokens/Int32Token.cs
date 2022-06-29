using System.Globalization;

namespace TauCode.Parsing.Tokens
{
    public class Int32Token : ValueTokenBase<int>
    {
        public Int32Token(
            int position,
            int consumedLength,
            int value)
            : base(
                position,
                consumedLength,
                value,
                value.ToString(CultureInfo.InvariantCulture))
        {
        }
    }
}
