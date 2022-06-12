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

// todo clean
public class SqlGraphScriptReader : GraphScriptReader
{
    //private readonly IScriptElementReader _wordVertexReader;
    //private readonly IScriptElementReader _identifierVertexReader;
    //private readonly IScriptElementReader _integerVertexReader;
    //private readonly IScriptElementReader _stringVertexReader;
    //private readonly IScriptElementReader _punctuationVertexReader;
    //private readonly IScriptElementReader _multiTextVertexReader;
    private readonly IScriptElementReader _alternativesGroupReader;
    private readonly IScriptElementReader _optionalGroupReader;
    //private readonly IScriptElementReader _idleVertexReader;
    //private readonly IScriptElementReader _endVertexReader;

    private readonly IScriptElementReader _vertexReader;

    public SqlGraphScriptReader()
    {
        _vertexReader = new VertexElementReader(this);

        //_wordVertexReader = new CustomVertexElementReader(
        //    this,
        //    new Dictionary<string, string>
        //    {
        //        { ":TYPE", "word" } // todo: and the word itself goes where? should be "exact-word" probably
        //    });
        //_identifierVertexReader = new CustomVertexElementReader(
        //    this,
        //    new Dictionary<string, string>
        //    {
        //        { ":TYPE", "identifier" }
        //    });
        //_integerVertexReader = new CustomVertexElementReader(
        //    this,
        //    new Dictionary<string, string>
        //    {
        //        { ":TYPE", "integer" }
        //    });
        //_stringVertexReader = new CustomVertexElementReader(
        //    this,
        //    new Dictionary<string, string>
        //    {
        //        { ":TYPE", "string" }
        //    });
        //_punctuationVertexReader = new CustomVertexElementReader(
        //    this,
        //    new Dictionary<string, string>
        //    {
        //        { ":TYPE", "punctuation" }
        //    });
        //_multiTextVertexReader = new CustomVertexElementReader(
        //    this,
        //    new Dictionary<string, string>
        //    {
        //        { ":TYPE", "multi-text" }
        //    });
        _alternativesGroupReader = new AlternativesGroupReader(this);
        _optionalGroupReader = new OptionalElementReader(this);
        //_idleVertexReader = new CustomVertexElementReader(
        //    this,
        //    new Dictionary<string, string>
        //    {
        //        { ":TYPE", "idle" }
        //    });
        //_endVertexReader = new CustomVertexElementReader(
        //    this,
        //    new Dictionary<string, string>
        //    {
        //        { ":TYPE", "end" }
        //    });
    }

    public override IScriptElementReader ResolveElementReader(Atom car)
    {
        if (car is Symbol symbol)
        {
            switch (symbol.Name)
            {
                case "IDENTIFIER":
                    //return _identifierVertexReader;

                case "INTEGER":
                    //return _integerVertexReader;

                case "STRING":
                    //return _stringVertexReader;

                case "MULTI-WORD":
                    //return _multiTextVertexReader;

                case "IDLE":
                    //return _idleVertexReader;

                case "END":
                    //return _endVertexReader;
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
                //return _punctuationVertexReader;
                return _vertexReader;
            }
            else if (stringAtom.Value.Length > 1)
            {
                //return _wordVertexReader;
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
