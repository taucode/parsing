using TauCode.Data.Graphs;
using TauCode.Parsing.Graphs.Molding;

namespace TauCode.Parsing.Graphs.Building
{
    public interface IGraphBuilder
    {
        IGraph Build(IGroupMold group);
    }
}
