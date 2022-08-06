using System.Globalization;

namespace TauCode.Parsing.Tokens;

public class Int64Token : ValueTokenBase<long>
{
    public Int64Token(
        int position,
        int consumedLength,
        long value)
        : base(
            position,
            consumedLength,
            value,
            value.ToString(CultureInfo.InvariantCulture))
    {
    }
}