using System;
using System.Collections.Generic;
using System.Text;

namespace Tau.Kappa.Parsing.LexicalTokens
{
    public class StringToken : TextToken
    {
        public StringToken(int position, int consumedLength, string text, string kind)
            : base(position, consumedLength, text)
        {
            this.Kind = kind;
        }

        public string Kind { get; }
    }
}
