using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TauCode.Data.Graphs;
using TauCode.Extensions;
using TauCode.Parsing.Graphs.Building;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.ParsingNodes;
using TauCode.Parsing.Tests.Parsing.Cli.Nodes;

namespace TauCode.Parsing.Tests.Graphs;

// todo clean
public class CliVertexFactory : IVertexFactory
{
    //public bool Accepts(IVertexMold vertexMold)
    //{
    //    var accepts = vertexMold.Type.IsIn(
    //        "term",
    //        "idle",
    //        "end",
    //        "key",
    //        "key-value");

    //    return accepts;
    //}

    public IVertex Create(IVertexMold vertexMold)
    {
        string alias;
        IParsingNode result;

        switch (vertexMold.Type)
        {
            case "term":
                var term = (string)vertexMold.KeywordValues["TERM"];
                result = new TermNode(term);
                break;

            case "key":
                var keyValues = (List<string>)vertexMold.KeywordValues["KEYS"]; // todo: don't emit ':'
                alias = (string)vertexMold.KeywordValues["ALIAS"];
                var isUnique = (bool)vertexMold.KeywordValues["IS-UNIQUE"];
                result = new KeyNode(keyValues, alias, isUnique);
                break;

            case "key-value":
                alias = (string)vertexMold.KeywordValues["ALIAS"];
                result = new KeyValueNode(alias);
                break;

            case "idle":
                result = new IdleNode();
                break;

            case "end":
                result =  new EndNode();
                break;

            default:
                throw new NotImplementedException();
        }

        result.Name = vertexMold.Name; // todo: check (in caller) that created node's name is equal to mold's name

        return result;
    }
}
