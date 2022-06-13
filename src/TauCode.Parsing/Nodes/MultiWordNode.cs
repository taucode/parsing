using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class MultiWordNode : ActionNode
    {
        public MultiWordNode(
            Action<ActionNode, ILexicalToken, IParsingResult> action,
            IEnumerable<string> values,
            bool isCaseSensitive)
            : base(action)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var valueList = values.ToList();

            if (valueList.Any(x => x == null))
            {
                throw new ArgumentException($"'{nameof(values)}' cannot contain nulls.", nameof(values));
            }

            if (!isCaseSensitive)
            {
                valueList = valueList
                    .Select(x => x.ToLowerInvariant())
                    .ToList();
            }

            this.Values = new HashSet<string>(valueList);
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

        protected override string GetDataTag()
        {
            var sb = new StringBuilder();

            var values = this.Values.ToList();

            for (var i = 0; i < values.Count; i++)
            {
                var value = values[i];
                sb.Append($"'{value}'");
                if (i < values.Count - 1)
                {
                    sb.Append(", ");
                }
            }

            return sb.ToString();
        }
    }
}
