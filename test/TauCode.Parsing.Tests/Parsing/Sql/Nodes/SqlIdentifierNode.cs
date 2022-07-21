using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Sql.Nodes;

public class SqlIdentifierNode : IdentifierNode
{
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
            if (SqlHelper.IsReservedWord(wordToken.Text))
            {
                return false;
            }

            return true;
        }

        return false;
    }
}
