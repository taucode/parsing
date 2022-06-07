using TauCode.Data.Graphs;
using TauCode.Parsing.Graphs.Molds;

namespace TauCode.Parsing.Graphs.Building.Impl
{
    public class VertexBuilder : IVertexBuilder
    {
        public IVertex Build(IVertexMold vertexMold)
        {
            throw new System.NotImplementedException();
        }

        public bool Accepts(IVertexMold vertexMold) => true;
    }
}
