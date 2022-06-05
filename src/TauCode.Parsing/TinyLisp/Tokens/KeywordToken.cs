using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TauCode.Parsing.TinyLisp.Tokens
{
    [DebuggerDisplay("{" + nameof(Keyword) + "}")]
    public class KeywordToken : LexicalTokenBase {
        public KeywordToken(
            int position,
            int consumedLength,
            string keyword)
            : base(position, consumedLength)
        {
            this.Keyword = keyword ?? throw new ArgumentNullException(nameof(keyword));
        }

        public string Keyword { get; }

        public override string ToString() => Keyword;
    }
}
