using TauCode.Parsing.Graphs.Reading;
using TauCode.Parsing.Graphs.Reading.Impl;

namespace TauCode.Parsing.Tests.Parsing.Sql.ScriptElementReaders;

public class WordVertexReader : VertexElementReader
{
    public WordVertexReader(IGraphScriptReader scriptReader)
        : base(scriptReader)
    {
    }
}
