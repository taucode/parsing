using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
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

        protected override string GetDataTag() => null;
    }
}
