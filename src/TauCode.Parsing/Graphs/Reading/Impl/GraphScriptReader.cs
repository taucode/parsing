using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading.Impl
{
    // todo clean
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
        private readonly IScriptElementReader _groupRefReader;

        #endregion

        #region ctor

        public GraphScriptReader()
        {
            _lexer = new TinyLispLexer();
            _lispReader = new TinyLispPseudoReader();

            _groupReader = new GroupElementReader(this);
            _sequenceReader = new SequenceElementReader(this);
            _splitterReader = new SplitterElementReader(this);
            _vertexReader = new VertexElementReader(this);
            _groupRefReader = new GroupRefElementReader(this);
        }

        #endregion

        #region Protected



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

                    case "GROUP-REF":
                        return _groupRefReader;

                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public virtual IGroupMold ReadScript(ReadOnlyMemory<char> script)
        {
            var tokens = _lexer.Tokenize(script);
            var scriptElement = _lispReader.Read(tokens);

            // todo: can throw
            var groupElement = scriptElement.Single();

            var groupReader = this.ResolveElementReader(groupElement.GetCar().AsElement<Atom>()); // todo: can throw
            // todo: check that groupReader is really group reader
            var group = groupReader.Read(null, groupElement);

            if (group is IGroupMold realGroup)
            {
                return realGroup;
            }

            throw new NotImplementedException();



            //var group = this.ReadGroup(null, groupElement);
            //return group;
        }

        #endregion
    }
}
