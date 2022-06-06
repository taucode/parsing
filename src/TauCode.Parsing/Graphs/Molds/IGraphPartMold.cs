using System.Collections.Generic;

namespace TauCode.Parsing.Graphs.Molds
{
    public interface IGraphPartMold
    {
        IGroupMold Owner { get; }
        IDictionary<string, object> Properties { get; }
    }
}
