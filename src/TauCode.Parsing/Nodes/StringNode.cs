using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class StringNode : ActionNode
    {
        public StringNode(
            Action<ActionNode, ParsingContext> action)
            : base(action)
        {
        }

        public StringNode()
        {
        }

        protected override bool AcceptsImpl(ParsingContext parsingContext)
        {
            var token = parsingContext.GetCurrentToken();
            return token is StringToken;
        }

        protected override string GetDataTag() => null;
    }
}
