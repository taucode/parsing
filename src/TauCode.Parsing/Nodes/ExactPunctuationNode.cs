using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class ExactPunctuationNode : ActionNode
    {
        public ExactPunctuationNode(
            Action<ActionNode, ParsingContext> action,
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

        protected override bool AcceptsImpl(ParsingContext parsingContext)
        {
            var token = parsingContext.GetCurrentToken();

            if (token is PunctuationToken punctuationToken)
            {
                return this.Punctuation == punctuationToken.Value;
            }

            return false;
        }

        protected override string GetDataTag() => this.Punctuation.ToString();
    }
}
