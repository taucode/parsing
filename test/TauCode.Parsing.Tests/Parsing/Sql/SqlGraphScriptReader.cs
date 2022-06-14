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
    private readonly IScriptElementReader _alternativesGroupReader;
    private readonly IScriptElementReader _optionalGroupReader;
    private readonly IScriptElementReader _vertexReader;

    public SqlGraphScriptReader()
    {
        _vertexReader = new VertexElementReader(this);
        _alternativesGroupReader = new AlternativesGroupReader(this);
        _optionalGroupReader = new OptionalElementReader(this);
    }

    public override IScriptElementReader ResolveElementReader(Atom car)
    {
        if (car is Symbol symbol)
        {
            switch (symbol.Name)
            {
                case "IDENTIFIER":
                case "INTEGER":
                case "STRING":
                case "MULTI-WORD":
                case "IDLE":
                case "END":
                    return _vertexReader;

                case "ALTERNATIVES":
                    return _alternativesGroupReader;

                case "OPTIONAL":
                    return _optionalGroupReader;

                default:
                    return base.ResolveElementReader(car);
            }
        }
        else if (car is StringAtom stringAtom)
        {
            if (stringAtom.Value.Length == 1)
            {
                return _vertexReader;
            }
            else if (stringAtom.Value.Length > 1)
            {
                return _vertexReader;
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
