using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Data;

namespace Tau.Kappa.Parsing.LexicalTokens
{
    public class EmailAddressToken : TextToken
    {
        public EmailAddressToken(
            int position,
            int consumedLength,
            EmailAddress emailAddress)
            : base(
                position,
                consumedLength,
                emailAddress.ToString())
        {
            this.EmailAddress = emailAddress;
        }

        public EmailAddress EmailAddress { get; }
    }
}
