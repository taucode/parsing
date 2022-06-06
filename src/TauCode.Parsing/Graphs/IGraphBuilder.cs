using TauCode.Data.Graphs;
using TauCode.Parsing.Graphs.Molds;

namespace TauCode.Parsing.Graphs
{
    public interface IGraphBuilder
    {
        IGraph Build(IGroupMold group);
    }
}
