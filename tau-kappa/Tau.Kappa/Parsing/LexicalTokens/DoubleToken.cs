using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Tau.Kappa.Parsing.LexicalTokens
{
    public class DoubleToken : TextToken
    {
        public DoubleToken(
            int position,
            int consumedLength,
            double value)
            : base(
                position,
                consumedLength,
                value.ToString(CultureInfo.InvariantCulture))
        {
            this.Value = value;
        }

        public double Value { get; }
    }
}
