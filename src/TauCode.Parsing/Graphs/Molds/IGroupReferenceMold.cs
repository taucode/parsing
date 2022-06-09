using System;
using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.Graphs.Molds
{
    public interface IGroupReferenceMold : IPartMold
    {
        string ReferencedGroupPath { get; set; }
    }
}
