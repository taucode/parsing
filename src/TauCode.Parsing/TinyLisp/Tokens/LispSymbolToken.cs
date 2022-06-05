using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TauCode.Parsing.TinyLisp.Tokens
{
    [DebuggerDisplay("{" + nameof(SymbolName) + "}")]
    public class LispSymbolToken : LexicalTokenBase
    {
        public LispSymbolToken(
            int position,
            int consumedLength,
            string symbolName)
            : base(position, consumedLength)
        {
            this.SymbolName = symbolName ?? throw new ArgumentNullException(nameof(symbolName));
        }

        public string SymbolName { get; }

        public override string ToString() => SymbolName;
    }
}
