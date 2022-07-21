namespace TauCode.Parsing.Tokens
{
    public class EnumToken<TEnum> : ValueTokenBase<TEnum> where TEnum : struct
    {
        public EnumToken(
            int position,
            int consumedLength,
            TEnum value)
            : base(
                position,
                consumedLength,
                value,
                value.ToString())
        {
        }
    }
}
