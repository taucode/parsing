using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TauCode.Parsing.LexicalTokens;

namespace TauCode.Parsing.ParsingNodes
{
    public class MultiWordNode : ActionNode
    {
        public MultiWordNode(
            Action<ActionNode, ILexicalToken, IParsingResult> action,
            IEnumerable<string> values,
            bool isCaseSensitive)
            : base(action)
        {
            // todo checks

            if (!isCaseSensitive)
            {
                values = values
                    .Select(x => x.ToLowerInvariant());
            }

            this.Values = new HashSet<string>(values);
            this.IsCaseSensitive = isCaseSensitive;
        }

        public bool IsCaseSensitive { get; }

        public MultiWordNode(
            IEnumerable<string> values,
            bool isCaseSensitive)
            : this(null, values, isCaseSensitive)
        {
        }

        public HashSet<string> Values { get; }

        protected override bool AcceptsTokenImpl(ILexicalToken token, IParsingResult parsingResult)
        {
            if (token is WordToken wordToken)
            {
                var value = wordToken.Text;
                if (!this.IsCaseSensitive)
                {
                    value = value.ToLowerInvariant();
                }

                return this.Values.Contains(value);
            }

            return false;
        }
    }
}
