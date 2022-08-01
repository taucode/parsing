using TauCode.Data.Graphs;
using TauCode.Parsing.Graphs.Molding;

namespace TauCode.Parsing.Graphs.Building;

public interface IVertexFactory
{
    IVertex Create(IVertexMold vertexMold);
}