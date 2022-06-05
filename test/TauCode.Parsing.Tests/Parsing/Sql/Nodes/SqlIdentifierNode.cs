using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TauCode.Parsing.LexicalTokens;
using TauCode.Parsing.ParsingNodes;

namespace TauCode.Parsing.Tests.Parsing.Sql.Nodes
{
    public class SqlIdentifierNode : IdentifierNode
    {
        protected override bool AcceptsTokenImpl(ILexicalToken token, IParsingResult parsingResult)
        {
            if (token is IdentifierToken)
            {
                return base.AcceptsTokenImpl(token, parsingResult);
            }

            if (token is WordToken wordToken)
            {
                if (SqlHelper.IsReservedWord(wordToken.Text))
                {
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
