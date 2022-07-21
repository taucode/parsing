using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class ExactWordNode : ActionNode
    {
        public ExactWordNode(
            Action<ActionNode, ParsingContext> action,
            string word,
            bool ignoreCase)
            : base(action)
        {
            this.Word = word ?? throw new ArgumentNullException(nameof(word));
            this.IgnoreCase = ignoreCase;
        }

        public ExactWordNode(
            string word,
            bool ignoreCase)
            : this(null, word, ignoreCase)
        {
        }

        public string Word { get; }

        public bool IgnoreCase { get; }

        protected override bool AcceptsImpl(ParsingContext parsingContext)
        {
            var token = parsingContext.GetCurrentToken();

            switch (token)
            {
                case WordToken wordToken:
                    {
                        var comparisonResult = string.Compare(
                            wordToken.Text,
                            this.Word,
                            this.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);

                        return comparisonResult == 0;
                    }
                default:
                    return false;
            }
        }

        protected override string GetDataTag() => $"'{this.Word}'";
    }
}
