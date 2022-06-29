using System.Globalization;

namespace TauCode.Parsing.Tokens
{
    public class BooleanToken : ValueTokenBase<bool>
    {
        public BooleanToken(
            int position,
            int consumedLength,
            bool value) :
            base(
                position,
                consumedLength,
                value,
                value.ToString(CultureInfo.InvariantCulture).ToLowerInvariant())
        {
        }
    }
}
