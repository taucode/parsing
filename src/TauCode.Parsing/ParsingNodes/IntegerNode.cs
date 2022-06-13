using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.LexicalTokens;

namespace TauCode.Parsing.ParsingNodes
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
    }
}
