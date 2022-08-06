namespace TauCode.Parsing.Tokens;

public class TimeSpanToken : ValueTokenBase<TimeSpan>
{
    public TimeSpanToken(
        int position,
        int consumedLength,
        TimeSpan value)
        : base(
            position,
            consumedLength,
            value,
            value.ToString("c"))
    {
    }
}