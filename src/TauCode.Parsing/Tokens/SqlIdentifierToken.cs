using TauCode.Data.Text;

namespace TauCode.Parsing.Tokens
{
    public class SqlIdentifierToken : ValueTokenBase<SqlIdentifier>
    {
        public SqlIdentifierToken(
            int position,
            int consumedLength,
            SqlIdentifier value)
            : base(
                position,
                consumedLength,
                value,
                value.ToString())
        {
        }
    }
}
