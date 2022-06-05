using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Data.Graphs;

namespace Tau.Kappa.Parsing
{
    // todo regions
    public abstract class ParsingNodeBase : Vertex, IParsingNode
    {
        protected abstract bool AcceptsTokenImpl(ILexicalToken token, IParsingResult parsingResult);
        protected abstract void ActImpl(ILexicalToken token, IParsingResult parsingResult);

        public bool AcceptsToken(ILexicalToken token, IParsingResult parsingResult)
        {
            // todo checks
            var result = this.AcceptsTokenImpl(token, parsingResult);
            return result;
        }

        public void Act(ILexicalToken token, IParsingResult parsingResult)
        {
            // todo checks
            this.ActImpl(token, parsingResult);
        }
    }
}
