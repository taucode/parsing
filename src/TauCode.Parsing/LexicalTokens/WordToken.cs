using System;
using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.LexicalTokens
{
    public class WordToken : TextToken
    {
        public WordToken(int position, int consumedLength, string text)
            : base(position, consumedLength, text)
        {
        }
    }
}
