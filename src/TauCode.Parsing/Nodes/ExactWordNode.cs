using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class ExactWordNode : ActionNode
    {
        public ExactWordNode(
            Action<ActionNode, ILexicalToken, IParsingResult> action,
            string word,
            bool isCaseSensitive)
            : base(action)
        {
            this.Word = word ?? throw new ArgumentNullException(nameof(word));
            this.IsCaseSensitive = isCaseSensitive;
        }

        public ExactWordNode(
            string word,
            bool isCaseSensitive)
            : this(null, word, isCaseSensitive)
        {   
        }

        public string Word { get; }

        public bool IsCaseSensitive { get; }

        protected override bool AcceptsTokenImpl(ILexicalToken token, IParsingResult parsingResult)
        {
            switch (token)
            {
                case WordToken wordToken:
                {
                    var comparisonResult = string.Compare(
                        wordToken.Text,
                        this.Word,
                        this.IsCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);

                    return comparisonResult == 0;
                }
                default:
                    return false;
            }
        }

        protected override string GetDataTag() => $"'{this.Word}'";
    }
}
