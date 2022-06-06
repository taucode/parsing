using System;
using TauCode.Parsing.Graphs.Molds;

namespace TauCode.Parsing.Graphs.Reading
{
    public interface IGraphScriptReader
    {
        IGroupMold ReadScript(ReadOnlyMemory<char> script);
    }
}
