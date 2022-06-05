﻿using TauCode.Data;

namespace TauCode.Parsing.LexicalTokens
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