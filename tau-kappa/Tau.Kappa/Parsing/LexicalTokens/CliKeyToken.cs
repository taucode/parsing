using System;
using System.Collections.Generic;
using System.Text;

namespace Tau.Kappa.Parsing.LexicalTokens
{
    public class CliKeyToken : TextToken
    {
        public CliKeyToken(int position, int consumedLength, string text)
            : base(position, consumedLength, text)
        {
        }
    }
}
