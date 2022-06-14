using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TauCode.Data.Graphs;
using TauCode.Extensions;
using TauCode.Parsing.Graphs.Building;
using TauCode.Parsing.Graphs.Molding;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tests.Parsing.Cli.Nodes;

namespace TauCode.Parsing.Tests.Graphs;

public class CliVertexFactory : IVertexFactory
{
    public IVertex Create(IVertexMold vertexMold)
    {
        string alias;
        IParsingNode result;

        switch (vertexMold.TypeAlias)
        {
            case "term":
                var term = (string)vertexMold.GetKeywordValue(":TERM");
                result = new TermNode(term);
                break;

            case "key":
                var keyValues = (List<string>)vertexMold.GetKeywordValue(":KEYS"); // todo: don't emit ':'
                alias = (string)vertexMold.GetKeywordValue(":ALIAS");
                var isUnique = (bool)vertexMold.GetKeywordValue(":IS-UNIQUE");
                result = new KeyNode(keyValues, alias, isUnique);
                break;

            case "key-value":
                alias = (string)vertexMold.GetKeywordValue(":ALIAS");
                result = new KeyValueNode(alias);
                break;

            case "idle":
                result = new IdleNode();
                break;

            case "end":
                result = new EndNode();
                break;

            default:
                throw new NotImplementedException();
        }

        result.Name = vertexMold.Name; // todo: check (in caller) that created node's name is equal to mold's name

        return result;
    }
}
