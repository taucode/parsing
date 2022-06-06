using System.Collections.Generic;

namespace TauCode.Parsing.Building.Molds
{
    public interface IGraphMold
    {
        IList<IGroupMold> Groups { get; }
    }
}
