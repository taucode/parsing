using System;

namespace TauCode.Parsing.ParsingNodes
{
    public sealed class EndNode : ParsingNodeBase
    {
        public EndNode()
        {   
        }

        protected override bool AcceptsTokenImpl(ILexicalToken token, IParsingResult parsingResult)
        {
            throw new NotImplementedException();
        }

        protected override void ActImpl(ILexicalToken token, IParsingResult parsingResult)
        {
            throw new NotImplementedException();
        }
    }
}
