using TauCode.Data.Graphs;

namespace TauCode.Parsing
{
    public static class ParsingExtensions
    {
        public static IEdge AddLink(this IParsingNode nodeFrom, IParsingNode nodeTo)
        {
            var edge = new Edge();
            edge.Connect(nodeFrom, nodeTo);

            return edge;
        }
    }
}
