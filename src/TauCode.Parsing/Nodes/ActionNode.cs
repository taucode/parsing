using System;

namespace TauCode.Parsing.Nodes
{
    public abstract class ActionNode : ParsingNodeBase
    {
        protected ActionNode(
            Action<ActionNode, ILexicalToken, IParsingResult> action)
        {
            this.Action = action;
        }

        protected ActionNode()
        {
        }

        public Action<ActionNode, ILexicalToken, IParsingResult> Action { get; set; }

        protected override void ActImpl(ILexicalToken token, IParsingResult parsingResult)
        {
            // todo: check this.Action is set.

            this.Action(this, token, parsingResult);
        }
    }
}
