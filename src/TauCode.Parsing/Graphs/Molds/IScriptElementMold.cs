using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molds
{
    public interface IScriptElementMold
    {
        IGroupMold Owner { get; }
        Atom Car { get; }
        string Name { get; set; }
        IDictionary<string, object> KeywordValues { get; }
    }
}
