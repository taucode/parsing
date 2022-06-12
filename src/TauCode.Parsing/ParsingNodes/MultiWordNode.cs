using System;
using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.ParsingNodes
{
    public class MultiWordNode : ActionNode
    {
        public MultiWordNode(
            Action<ActionNode, ILexicalToken, IParsingResult> action,
            IEnumerable<string> values)
            : base(action)
        {
            // todo checks
            this.Values = new HashSet<string>(values);
        }

        public MultiWordNode(IEnumerable<string> values)
            : this(null, values)
        {
        }

        public HashSet<string> Values { get; }

        protected override bool AcceptsTokenImpl(ILexicalToken token, IParsingResult parsingResult)
        {
            throw new NotImplementedException();
        }
    }
}
