using System;

namespace TauCode.Parsing.Nodes
{
    public abstract class ActionNode : ParsingNodeBase
    {
        protected ActionNode(
            Action<ActionNode, ParsingContext> action)
        {
            this.Action = action;
        }

        protected ActionNode()
        {
        }

        public Action<ActionNode, ParsingContext> Action { get; set; }

        protected override void ActImpl(ParsingContext parsingContext)
        {
            // todo: check this.Action is set.

            this.Action(this, parsingContext);
        }
    }
}
