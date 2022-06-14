using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class IntegerNode : ActionNode
    {
        public IntegerNode(
            Action<ActionNode, ILexicalToken, IParsingResult> action)
            : base(action)
        {
        }

        public IntegerNode()
        {
        }

        protected override bool AcceptsTokenImpl(ILexicalToken token, IParsingResult parsingResult)
        {
            return token is IntegerToken;
        }

        protected override string GetDataTag() => null;
    }
}
