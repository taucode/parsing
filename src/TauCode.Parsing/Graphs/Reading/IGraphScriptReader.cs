using System;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading
{
    public interface IGraphScriptReader
    {
        IElementReader ResolveElementReader(Atom car);
        IGroupMold ReadScript(ReadOnlyMemory<char> script);
    }
}
