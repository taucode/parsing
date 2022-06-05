using System;
using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.Building
{
    public interface IVertexMold : IGraphPartMold
    {
        string Name { get; set; }

        string GetFullName();

        IGroupMold Owner { get; }
    }
}
