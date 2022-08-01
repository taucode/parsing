using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Graphs.Molding;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

// todo clean
namespace TauCode.Parsing.Graphs.Reading.Impl;

public class GraphScriptReader : IGraphScriptReader
{
    #region Constants & Invariants

    private static readonly HashSet<string> VertexKeywords = new HashSet<string>
    {
        ":NAME",
        ":TYPE",
    };

    #endregion

    #region Fields

    private readonly ILexer _lexer;
    private readonly ITinyLispPseudoReader _lispReader;

    private readonly IScriptElementReader _groupReader;
    private readonly IScriptElementReader _sequenceReader;
    private readonly IScriptElementReader _splitterReader;
    private readonly IScriptElementReader _vertexReader;
    //private readonly IScriptElementReader _groupRefReader;
    private readonly IScriptElementReader _refReader;
    private readonly IScriptElementReader _arcReader;

    #endregion

    #region ctor

    public GraphScriptReader()
    {
        _lexer = new TinyLispLexer();
        _lispReader = new TinyLispPseudoReader();

        _groupReader = new GroupReader(this);
        _sequenceReader = new SequenceReader(this);
        _splitterReader = new SplitterReader(this);
        _vertexReader = new VertexReader(this);
        _refReader = new RefReader(this);
        //_groupRefReader = new GroupRefReader(this);
        _arcReader = new ArcReader(this);
    }

    #endregion

    #region IGraphScriptReader Members

    public virtual IScriptElementReader ResolveElementReader(Atom car)
    {
        if (car is Symbol symbol)
        {
            switch (symbol.Name)
            {
                case "GROUP":
                    return _groupReader;

                case "SEQUENCE":
                    return _sequenceReader;

                case "SPLITTER":
                    return _splitterReader;

                case "VERTEX":
                    return _vertexReader;

                case "REF":
                    return _refReader;

                case "ARC":
                    return _arcReader;
            }
        }

        throw new ReadingException($"Unexpected element to read: '{car}'.");
    }

    public virtual IGroupMold ReadScript(ReadOnlyMemory<char> script)
    {
        var tokens = _lexer.Tokenize(script);
        var scriptElement = _lispReader.Read(tokens);

        if (scriptElement.Count != 1)
        {
            throw new ReadingException("Script should contain exactly one group at the top level.");
        }

        var groupElement = scriptElement.Single();
        var groupReader = this.ResolveElementReader(groupElement.GetCar<Atom>());
        var group = groupReader.Read(null, groupElement);

        if (group is IGroupMold realGroup)
        {
            return realGroup;
        }

        throw new ReadingException("Could not read script.");
    }

    #endregion
}