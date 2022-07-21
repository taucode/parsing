using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class IdentifierNode : ActionNode
    {
        public IdentifierNode(
            Action<ActionNode, ParsingContext> action)
            : base(action)
        {
        }

        public IdentifierNode()
        {   
        }

        protected override bool AcceptsImpl(ParsingContext parsingContext)
        {
            var token = parsingContext.GetCurrentToken();

            if (token is IdentifierToken)
            {
                return true;
            }

            return false;
        }

        protected override string GetDataTag() => null;
    }
}
