using TauCode.Data.Text;

namespace TauCode.Parsing.Tokens
{
    public class EmailAddressToken : ValueTokenBase<EmailAddress>
    {
        public EmailAddressToken(
            int position,
            int consumedLength,
            EmailAddress emailAddress)
            : base(
                position,
                consumedLength,
                emailAddress,
                emailAddress.ToString()!)
        {
        }
    }
}
