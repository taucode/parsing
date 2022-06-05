using System;
using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.Building
{
    public interface IGraphMold
    {
        IList<IGraphPartMold> Parts { get; }
    }
}
