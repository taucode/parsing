using System.Collections.Generic;
using TauCode.Data.Graphs;
using TauCode.Parsing.Graphs.Molds;

namespace TauCode.Parsing.Graphs.Building
{
    public interface IGraphBuilder
    {
        IEnumerable<IVertexBuilder> CustomVertexBuilders { get; set; }
        IEnumerable<IArcBuilder> CustomArcBuilders { get; set; }
        IGraph Build(IGroupMold group);
    }
}
