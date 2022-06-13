using System.Collections.Generic;

namespace TauCode.Parsing
{
    public class ParsingContext
    {
        public ParsingContext(IReadOnlyList<ILexicalToken> tokens)
        {
            this.Tokens = tokens;
        }

        public readonly IReadOnlyList<ILexicalToken> Tokens;
        public int Position;
    }
}
