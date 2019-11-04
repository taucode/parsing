﻿namespace TauCode.Parsing.Tests.Tokens
{
    public class IdentifierToken : IToken
    {
        public IdentifierToken(string identifier)
        {
            this.Identifier = identifier;
        }

        public string Identifier { get; }
    }
}
