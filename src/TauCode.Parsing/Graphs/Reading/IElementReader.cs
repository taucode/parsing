using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading
{
    public interface IElementReader
    {
        IGraphScriptReader ScriptReader { get; }
        IPartMold Read(IGroupMold owner, Element element);
    }
}
