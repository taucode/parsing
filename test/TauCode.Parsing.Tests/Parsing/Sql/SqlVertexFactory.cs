using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TauCode.Data.Graphs;
using TauCode.Parsing.Graphs.Building;
using TauCode.Parsing.Graphs.Building.Impl;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.ParsingNodes;
using TauCode.Parsing.Tests.Parsing.Sql.Nodes;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Tests.Parsing.Sql;

public class SqlVertexFactory : IVertexFactory
{
    public IVertex Create(IVertexMold vertexMold)
    {
        IVertex result;

        if (vertexMold.Car is StringAtom stringAtom)
        {
            var value = stringAtom.Value;
            if (value.Length == 1)
            {
                result = new ExactPunctuationNode(value.Single());
            }
            else
            {
                result = new ExactWordNode(value, false);
            }
        }
        else if (vertexMold.Car is Symbol symbol)
        {
            switch (symbol.Name)
            {
                case "IDLE":
                    result = new IdleNode();
                    break;

                case "END":
                    result = new EndNode();
                    break;

                case "IDENTIFIER":
                    result = new SqlIdentifierNode();
                    break;

                case "INTEGER":
                    result = new IntegerNode();
                    break;

                case "STRING":
                    result = new StringNode();
                    break;

                case "MULTI-WORD":
                    result = new MultiWordNode((List<string>)vertexMold.GetKeywordValue(":VALUES"), false);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
        else
        {
            throw new NotImplementedException();
        }

        if (result is ActionNode actionNode)
        {
            actionNode.Action = IdleAction;
        }

        result.Name = vertexMold.Name;
        return result;
    }

    private static void IdleAction(ActionNode node, ILexicalToken token, IParsingResult parsingResult)
    {
        SqlParsingResult sqlParsingResult = (SqlParsingResult)parsingResult;
        sqlParsingResult.IncreaseVersion();
    }
}
