using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.LexicalTokens;

namespace TauCode.Parsing.ParsingNodes
{
    public class IdentifierNode : ActionNode
    {
        public IdentifierNode(
            Action<ActionNode, ILexicalToken, IParsingResult> action)
            : base(action)
        {
        }

        public IdentifierNode()
        {   
        }

        protected override bool AcceptsTokenImpl(ILexicalToken token, IParsingResult parsingResult)
        {
            if (token is IdentifierToken)
            {
                return true;
            }

            return false;
        }
    }
}
