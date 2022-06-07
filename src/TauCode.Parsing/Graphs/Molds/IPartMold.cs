using System.Collections.Generic;

namespace TauCode.Parsing.Graphs.Molds
{
    public interface IPartMold
    {
        IGroupMold Owner { get; }
        IDictionary<string, object> Properties { get; }
        IVertexMold Entrance { get; }
        IVertexMold Exit { get; }
    }
}
