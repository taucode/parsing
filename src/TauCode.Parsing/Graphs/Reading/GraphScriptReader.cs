using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.Graphs.Molds.Impl;
using TauCode.Parsing.Graphs.Reading.ElementReaders;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading
{
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

        private readonly IElementReader _sequenceReader;
        private readonly IElementReader _splitterReader;
        private readonly IElementReader _vertexReader;

        #endregion

        #region ctor

        public GraphScriptReader()
        {
            _lexer = new TinyLispLexer();
            _lispReader = new TinyLispPseudoReader();

            _sequenceReader = new SequenceElementReader(this);
            _splitterReader = new SplitterElementReader(this);
            _vertexReader = new VertexElementReader(this);
        }

        #endregion

        #region Protected



        #endregion

        #region IGraphScriptReader Members

        public virtual IElementReader ResolveElementReader(Atom car)
        {
            if (car is Symbol symbol)
            {
                switch (symbol.Name)
                {
                    case "SEQUENCE":
                        return _sequenceReader;

                    case "SPLITTER":
                        return _splitterReader;

                    case "VERTEX":
                        return _vertexReader;

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

            var groupReader = this.ResolveElementReader(groupElement.GetCar().AsElement<Atom>());
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



        //private IGroupMold ReadGroup(IGroupMold owner, Element groupElement)
        //{
        //    var carElement = groupElement.GetCar();

        //    if (carElement is Symbol symbol)
        //    {
        //        switch (symbol.Name.ToLowerInvariant())
        //        {
        //            case "sequence":
        //                return this.ReadSequence(owner, groupElement);

        //            default:
        //                throw new NotImplementedException();
        //        }
        //    }
        //    else
        //    {
        //        throw new NotImplementedException();
        //    }

        //    throw new NotImplementedException();
        //}

        //private SequenceMold ReadSequence(IGroupMold owner, Element groupElement)
        //{
        //    var sequenceMold = new SequenceMold(owner);

        //    var name = groupElement.GetSingleKeywordArgument<StringAtom>(":name", true)?.Value;
        //    sequenceMold.Name = name;

        //    var content = groupElement.GetFreeArguments();

        //    foreach (var element in content)
        //    {
        //        var part = this.ReadPart(sequenceMold, element);
        //        sequenceMold.Add(part);
        //    }

        //    return sequenceMold;
        //}

        //private IPartMold ReadPart(IGroupMold owner, Element element)
        //{
        //    var carElement = element.GetCar();

        //    if (carElement is Symbol symbol)
        //    {
        //        switch (symbol.Name)
        //        {
        //            case "VERTEX":
        //                return this.ReadVertex(owner, element);

        //            case "SPLITTER":
        //                return this.ReadSplitter(owner, element);

        //            default:
        //                throw new NotImplementedException();
        //        }
        //    }
        //    else
        //    {
        //        throw new NotImplementedException();
        //    }

        //    throw new NotImplementedException();
        //}

        //private IVertexMold ReadVertex(IGroupMold owner, Element vertexElement)
        //{
        //    var pseudoList = (PseudoList)vertexElement; // todo can throw?
        //    var vertexMold = new VertexMold(owner);

        //    var name = vertexElement.GetSingleKeywordArgument<StringAtom>(":name", true)?.Value;
        //    var type = vertexElement.GetSingleKeywordArgument<StringAtom>(":type", true)?.Value;

        //    vertexMold.Name = name;
        //    vertexMold.Type = type;

        //    var keywords = pseudoList
        //        .Where(x => x is Keyword)
        //        .Cast<Keyword>()
        //        .ToList();

        //    foreach (var keyword in keywords)
        //    {
        //        var keywordName = keyword.Name;

        //        if (keywordName.StartsWith(":@"))
        //        {
        //            var keyValueElement = pseudoList.GetSingleKeywordArgument(keywordName);
        //            this.AddProperty(vertexMold, keywordName, keyValueElement);
        //        }
        //        else
        //        {
        //            if (VertexKeywords.Contains(keywordName))
        //            {
        //                // ok
        //            }
        //            else
        //            {
        //                throw new NotImplementedException();
        //            }
        //        }
        //    }

        //    return vertexMold;
        //}

        //private void AddProperty(IPartMold part, string keywordName, Element keyValueElement)
        //{
        //    var name = keywordName[2..].ToLowerInvariant(); // todo: keyword ':@' (which is valid) will fail
        //    var value = this.TransformPropertyValue(keyValueElement);

        //    part.Properties.Add(name, value);
        //}

        //private object TransformPropertyValue(Element keyValueElement)
        //{
        //    if (keyValueElement is StringAtom stringAtom)
        //    {
        //        return stringAtom.Value;
        //    }
        //    else
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
    }
}
