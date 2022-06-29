using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TauCode.Parsing.Nodes
{
    public class TextNode : ActionNode
    {
        public TextNode(
            IEnumerable<Type> tokenTypes)
        {
            // todo checks

            this.TokenTypes = new HashSet<Type>(tokenTypes);
        }

        public HashSet<Type> TokenTypes { get; }

        protected override bool AcceptsImpl(ParsingContext parsingContext)
        {
            var token = parsingContext.GetCurrentToken();

            if (this.TokenTypes.Contains(token.GetType()))
            {
                return true;
            }

            if (this.TokenConverter == null)
            {
                return false;
            }

            return this.TokenTypes
                .Select(tokenType => this.TokenConverter.Convert(token, tokenType, parsingContext.ParsingResult))
                .Any(convertedToken => convertedToken != null);
        }

        protected override string GetDataTag() => null;
    }
}
