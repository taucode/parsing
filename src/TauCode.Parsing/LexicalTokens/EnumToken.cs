namespace TauCode.Parsing.LexicalTokens
{
    public class EnumToken<TEnum> : LexicalTokenBase where TEnum : struct
    {
        #region Constructor

        public EnumToken(
            int position,
            int consumedLength,
            TEnum value)
            : base(position, consumedLength)
        {
            this.Value = value;
        }

        #endregion

        #region Public

        public TEnum Value { get; }

        #endregion

        #region Overridden

        public override string ToString() => this.Value.ToString();

        #endregion
    }
}
