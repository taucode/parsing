using System.Collections.Generic;

namespace Tau.Kappa.Parsing
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
