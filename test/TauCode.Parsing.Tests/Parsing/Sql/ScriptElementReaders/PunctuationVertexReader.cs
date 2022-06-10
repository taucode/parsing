using TauCode.Parsing.Graphs.Reading;
using TauCode.Parsing.Graphs.Reading.Impl;

namespace TauCode.Parsing.Tests.Parsing.Sql.ScriptElementReaders;

public class PunctuationVertexReader : VertexElementReader
{
    public PunctuationVertexReader(IGraphScriptReader scriptReader)
        : base(scriptReader)
    {
    }
}
