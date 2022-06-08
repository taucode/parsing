using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading.Impl
{
    public abstract class ElementReaderBase : IElementReader
    {
        protected ElementReaderBase(IGraphScriptReader scriptReader)
        {
            this.ScriptReader = scriptReader ?? throw new ArgumentNullException(nameof(scriptReader));
        }

        protected abstract IPartMold CreatePartMold(IGroupMold owner);

        protected abstract void ReadContent(Element element, IPartMold partMold);

        protected abstract void ValidateResult(Element element, IPartMold partMold);

        protected virtual void ReadKeywordValues(Element element, IPartMold partMold)
        {
            var keywordValues = element.GetAllKeywordArguments();

            foreach (var tuple in keywordValues)
            {
                var keyword = tuple.Item2;
                var keywordValue = tuple.Item3;

                if (keyword.Name.StartsWith(":@"))
                {
                    this.AddProperty(partMold, keyword.Name, keywordValue);
                }
                else
                {
                    this.ProcessBasicKeyword(partMold, keyword.Name, keywordValue);
                }
            }
        }

        private void AddProperty(IPartMold partMold, string keywordName, Element keywordValue)
        {
            var dictionaryKey = keywordName.Substring(2);
            object dictionaryValue;
            if (keywordValue is StringAtom stringAtom)
            {
                dictionaryValue = stringAtom.Value;
            }
            else if (keywordValue is True || keywordValue is Nil)
            {
                dictionaryValue = keywordValue.ToBool();
            }
            else if (keywordValue is PseudoList pseudoList)
            {
                if (pseudoList.Count == 0)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    var first = pseudoList[0];

                    if (first is StringAtom)
                    {
                        dictionaryValue = PseudoListToStringList(pseudoList);
                    }
                    else if (first is Keyword)
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            partMold.Properties.Add(dictionaryKey, dictionaryValue);
        }

        protected static List<string> PseudoListToStringList(Element element)
        {
            // todo can throw
            var pseudoList = (PseudoList)element;

            // todo can throw
            return pseudoList
                .Select(x => ((StringAtom)x).Value)
                .ToList();
        }

        private bool IsPropertyKeyword(string keywordName)
        {
            throw new NotImplementedException();
        }

        protected virtual void ProcessBasicKeyword(IPartMold partMold, string keywordName, Element keywordValue)
        {
            switch (keywordName)
            {
                case ":NAME":
                    if (keywordValue is StringAtom stringAtom)
                    {
                        partMold.Name = stringAtom.Value;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                    break;

                case ":IS-ENTRANCE":
                    partMold.IsEntrance = keywordValue.ToBool();
                    break;

                case ":IS-EXIT":
                    partMold.IsExit = keywordValue.ToBool();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public IGraphScriptReader ScriptReader { get; }

        public IPartMold Read(IGroupMold owner, Element element)
        {
            var partMold = this.CreatePartMold(owner);

            this.ReadKeywordValues(element, partMold);
            this.ReadContent(element, partMold);
            this.ValidateResult(element, partMold);

            return partMold;
        }
    }
}
