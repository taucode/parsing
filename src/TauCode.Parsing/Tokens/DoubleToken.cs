using System.Globalization;

namespace TauCode.Parsing.Tokens;

public class DoubleToken : ValueTokenBase<double>
{
    public DoubleToken(
        int position,
        int consumedLength,
        double value)
        : base(
            position,
            consumedLength,
            value,
            value.ToString(CultureInfo.InvariantCulture))
    {
    }
}