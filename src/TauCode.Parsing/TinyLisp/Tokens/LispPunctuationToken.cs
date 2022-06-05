using System;
using System.Diagnostics;
using TauCode.Parsing.LexicalTokens;

namespace TauCode.Parsing.TinyLisp.Tokens
{
    [DebuggerDisplay("{" + nameof(ToString) + "()}")]
    public class LispPunctuationToken : EnumToken<Punctuation>
    {
        public LispPunctuationToken(
            int position,
            int consumedLength,
            Punctuation value)
            : base(position, consumedLength, value)
        {
            if (consumedLength != 1)
            {
                throw new ArgumentOutOfRangeException(nameof(consumedLength));
            }
        }

        public override string ToString() => Value.PunctuationToChar().ToString();
    }
}
