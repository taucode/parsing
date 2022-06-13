using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.LexicalTokens;

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
            if (token is PunctuationToken punctuationToken)
            {
                return this.Punctuation == punctuationToken.Value;
            }

            return false;
        }
    }
}
