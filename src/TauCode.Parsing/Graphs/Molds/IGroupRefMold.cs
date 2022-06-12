using System;
using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.Graphs.Molds
{
    public interface IGroupRefMold : ILinkableMold
    {
        string ReferencedGroupPath { get; set; }
    }
}
