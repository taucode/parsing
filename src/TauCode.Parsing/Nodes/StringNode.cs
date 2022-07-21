using System;

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
            throw new NotImplementedException();
        }

        protected override string GetDataTag() => null;
    }
}
