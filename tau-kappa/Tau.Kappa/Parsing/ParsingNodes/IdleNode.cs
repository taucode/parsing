using System;
using System.Collections.Generic;
using System.Text;

namespace Tau.Kappa.Parsing.ParsingNodes
{
    public class IdleNode : ParsingNodeBase
    {
        protected override bool AcceptsTokenImpl(ILexicalToken token, IParsingResult parsingResult) => true;

        protected override void ActImpl(ILexicalToken token, IParsingResult parsingResult)
        {
            // idle
        }
    }
}
