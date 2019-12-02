﻿using System;
using System.Collections.Generic;
using TauCode.Parsing.Lexizing;

namespace TauCode.Parsing.Tokens
{
    public class PunctuationToken : TokenBase
    {
        public PunctuationToken(
            char c,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(name, properties)
        {
            if (!LexerHelper.IsStandardPunctuationChar(c))
            {
                throw new ArgumentOutOfRangeException(nameof(c));
            }
        }
    }
}