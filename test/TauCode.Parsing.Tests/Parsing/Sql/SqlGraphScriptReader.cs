using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TauCode.Parsing.Graphs.Reading;
using TauCode.Parsing.Graphs.Reading.Impl;
using TauCode.Parsing.Tests.Parsing.Sql.ScriptElementReaders;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Tests.Parsing.Sql;

public class SqlGraphScriptReader : GraphScriptReader
{
    private readonly IScriptElementReader _wordVertexReader;
    private readonly IScriptElementReader _punctuationVertexReader;
    private readonly IScriptElementReader _multiTextVertexReader;
    private readonly IScriptElementReader _alternativesGroupReader;
    private readonly IScriptElementReader _idleVertexReader;
    private readonly IScriptElementReader _endVertexReader;

    public SqlGraphScriptReader()
    {
        _wordVertexReader = new WordVertexReader(this);
        _punctuationVertexReader = new PunctuationVertexReader(this);
        _multiTextVertexReader = new MultiTextVertexReader(this);
        _alternativesGroupReader = new AlternativesGroupReader(this);
        _idleVertexReader = new CustomVertexElementReader(this, new Dictionary<string, string>
        {
            { ":TYPE", "idle" }
        });
        _endVertexReader = new CustomVertexElementReader(this, new Dictionary<string, string>
        {
            { ":TYPE", "end" }
        });
    }

    public override IScriptElementReader ResolveElementReader(Atom car)
    {
        if (car is Symbol symbol)
        {
            switch (symbol.Name)
            {
                case "ALTERNATIVES":
                    return _alternativesGroupReader;

                case "IDLE":
                    return _idleVertexReader;

                case "END":
                    return _endVertexReader;

                default:
                    return base.ResolveElementReader(car);
            }
        }
        else if (car is StringAtom stringAtom)
        {
            if (stringAtom.Value.Length == 1)
            {
                throw new NotImplementedException();
            }
            else if (stringAtom.Value.Length > 1)
            {
                return _wordVertexReader;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        else
        {
            throw new NotImplementedException($"error: unexpected car: '{car}'.");
        }
    }
}
