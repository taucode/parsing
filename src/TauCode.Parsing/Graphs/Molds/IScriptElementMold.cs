using System;
using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.Graphs.Molds
{
    public interface IScriptElementMold
    {
        IGroupMold Owner { get; }
        string Name { get; set; }
        IDictionary<string, object> Properties { get; }
    }
}
