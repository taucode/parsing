using System;

namespace TauCode.Parsing.Tokens
{
    public class DateTimeOffsetToken : ValueTokenBase<DateTimeOffset>
    {
        public DateTimeOffsetToken(
            int position,
            int consumedLength,
            DateTimeOffset value)
            : base(
                position,
                consumedLength,
                value,
                value.ToString("o"))
        {
        }
    }
}
