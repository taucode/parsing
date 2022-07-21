using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.Graphs.Molding;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading
{
    public interface IScriptElementReader
    {
        IGraphScriptReader ScriptReader { get; }
        IScriptElementMold Read(IGroupMold owner, Element element);
    }
}
