using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class SqlIdentifierNode : IdentifierNode
    {
        public SqlIdentifierNode(
            Action<ActionNode, ParsingContext> action,
            Func<string, bool> isReservedWordPredicate)
            : base(action)
        {
            this.IsReservedWordPredicate = isReservedWordPredicate ?? throw new ArgumentNullException(nameof(isReservedWordPredicate));
        }

        public SqlIdentifierNode(
            Func<string, bool> isReservedWordPredicate)
        {
            this.IsReservedWordPredicate = isReservedWordPredicate ?? throw new ArgumentNullException(nameof(isReservedWordPredicate));
        }

        public Func<string, bool> IsReservedWordPredicate { get; }


        protected override bool AcceptsImpl(ParsingContext parsingContext)
        {
            var token = parsingContext.GetCurrentToken();

            if (token is SqlIdentifierToken)
            {
                return true;
            }

            if (token is IdentifierToken)
            {
                return base.AcceptsImpl(parsingContext);
            }

            if (token is WordToken wordToken)
            {
                if (this.IsReservedWordPredicate(wordToken.Text))
                {
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
