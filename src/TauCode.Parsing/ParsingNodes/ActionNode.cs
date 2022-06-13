using System;

namespace TauCode.Parsing.ParsingNodes
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
            this.Action(this, token, parsingResult);
        }
    }
}
