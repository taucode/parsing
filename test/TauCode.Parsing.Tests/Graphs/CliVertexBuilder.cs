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

public class CliVertexBuilder : IVertexBuilder
{
    public bool Accepts(IVertexMold vertexMold)
    {
        var accepts = vertexMold.Type.IsIn(
            "term",
            "idle",
            "end",
            "key",
            "key-value");

        if (!accepts)
        {
            throw new NotImplementedException();
        }

        return accepts;
    }

    public IVertex Build(IVertexMold vertexMold)
    {
        string alias;

        switch (vertexMold.Type)
        {
            case "term":
                var term = (string)vertexMold.Properties["TERM"];
                var termNode = new TermNode(term);
                return termNode;

            case "key":
                var keyValues = (List<string>)vertexMold.Properties["KEYS"];
                alias = (string)vertexMold.Properties["ALIAS"];
                var isUnique = (bool)vertexMold.Properties["IS-UNIQUE"];
                var keyNode = new KeyNode(keyValues, alias, isUnique);
                return keyNode;

            case "key-value":
                alias = (string)vertexMold.Properties["ALIAS"];
                var keyValueNode = new KeyValueNode(alias);
                return keyValueNode;

            case "idle":
                var idleNode = new IdleNode();
                return idleNode;

            case "end":
                return EndNode.Instance;

            default:
                throw new NotImplementedException();
        }
    }
}
