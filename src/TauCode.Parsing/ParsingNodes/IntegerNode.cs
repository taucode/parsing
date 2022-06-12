using System;
using System.Collections.Generic;
using System.Text;

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
            throw new NotImplementedException();
        }
    }
}
