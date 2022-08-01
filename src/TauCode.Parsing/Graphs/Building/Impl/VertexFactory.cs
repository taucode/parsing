using TauCode.Data.Graphs;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Graphs.Molding;

namespace TauCode.Parsing.Graphs.Building.Impl;

public class VertexFactory : IVertexFactory
{
    public IVertex Create(IVertexMold vertexMold)
    {
        if (vertexMold.TypeAlias == null)
        {
            var vertex = new Vertex(vertexMold.Name!);
            return vertex;
        }

        throw new BuildingException($"Unknown type alias: '{vertexMold.TypeAlias}'.");
    }
}