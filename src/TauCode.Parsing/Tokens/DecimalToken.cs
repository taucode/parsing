using System.Globalization;

namespace TauCode.Parsing.Tokens;

public class DecimalToken : ValueTokenBase<decimal>
{
    public DecimalToken(
        int position,
        int consumedLength,
        decimal value)
        : base(
            position,
            consumedLength,
            value,
            value.ToString(CultureInfo.InvariantCulture))
    {
    }
}