using System;
using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.ParsingNodes
{
    public class ExactPunctuationNode : ActionNode
    {
        public ExactPunctuationNode(
            Action<ActionNode, ILexicalToken, IParsingResult> action,
            char punctuation)
            : base(action)
        {
            this.Punctuation = punctuation;
        }

        public ExactPunctuationNode(char punctuation)
            : this(null, punctuation)
        {
        }

        public char Punctuation { get; }

        protected override bool AcceptsTokenImpl(ILexicalToken token, IParsingResult parsingResult)
        {
            throw new NotImplementedException();
        }
    }
}
