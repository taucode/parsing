using System;
using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.Building
{
    public interface IGroupMold : IGraphPartMold
    {
        string GroupName { get; }

        ISet<IGraphPartMold> Content { get; }

        IVertexMold Entry { get; set; }

        IVertexMold Exit { get; set; }
    }
}
