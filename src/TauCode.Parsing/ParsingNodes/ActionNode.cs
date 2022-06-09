using System;

namespace TauCode.Parsing.ParsingNodes
{
    public abstract class ActionNode : ParsingNodeBase
    {
        protected ActionNode(
            Action<ActionNode, ILexicalToken, IParsingResult> actCallback)
        {
            this.ActCallback = actCallback;
        }

        public Action<ActionNode, ILexicalToken, IParsingResult> ActCallback { get; set; }

        protected override void ActImpl(ILexicalToken token, IParsingResult parsingResult)
        {
            this.ActCallback(this, token, parsingResult);
        }
    }
}
