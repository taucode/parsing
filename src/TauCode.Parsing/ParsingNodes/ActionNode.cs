using System;

namespace TauCode.Parsing.ParsingNodes
{
    public class ActionNode : ParsingNodeBase
    {
        public ActionNode()
        {   
        }

        public ActionNode(
            Func<ActionNode, ILexicalToken, IParsingResult, bool> acceptPredicate,
            Action<ActionNode, ILexicalToken, IParsingResult> actCallback)
        {
            this.AcceptPredicate = acceptPredicate;
            this.ActCallback = actCallback;
        }

        public Func<ActionNode, ILexicalToken, IParsingResult, bool> AcceptPredicate { get; set; }
        public Action<ActionNode, ILexicalToken, IParsingResult> ActCallback { get; set; }

        protected override bool AcceptsTokenImpl(ILexicalToken token, IParsingResult parsingResult)
        {
            return AcceptPredicate(this, token, parsingResult);
        }

        protected override void ActImpl(ILexicalToken token, IParsingResult parsingResult)
        {
            this.ActCallback(this, token, parsingResult);
        }
    }
}
