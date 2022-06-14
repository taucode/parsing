using System;

namespace TauCode.Parsing.Nodes
{
    public class StringNode : ActionNode
    {
        public StringNode(
            Action<ActionNode, ILexicalToken, IParsingResult> action)
            : base(action)
        {
        }

        public StringNode()
        {
        }

        protected override bool AcceptsTokenImpl(ILexicalToken token, IParsingResult parsingResult)
        {
            throw new NotImplementedException();
        }

        protected override string GetDataTag() => null;
    }
}
