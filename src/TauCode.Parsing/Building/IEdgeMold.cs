using System;
using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.Building
{
    public interface IEdgeMold : IGraphPartMold
    {
        IVertexMold Tail { get; set; }
        string TailFullName { get; set; }
        IVertexMold Head { get; set; }
        string HeadFullName { get; set; }
    }
}
