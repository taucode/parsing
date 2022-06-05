using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Tau.Kappa.Parsing.LexicalTokens
{
    public class IntegerToken : TextToken
    {
        public IntegerToken(
            int position,
            int consumedLength,
            long value) :
            base(
                position,
                consumedLength,
                value.ToString(CultureInfo.InvariantCulture))
        {
            this.Value = value;
        }

        public long Value { get; }
    }
}
