using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading.Impl
{
    // todo regions, clean
    public abstract class ScriptElementReaderBase : IScriptElementReader
    {
        protected ScriptElementReaderBase(IGraphScriptReader scriptReader)
        {
            this.ScriptReader = scriptReader ?? throw new ArgumentNullException(nameof(scriptReader));
        }

        protected abstract IScriptElementMold CreateScriptElementMold(IGroupMold owner, Element element);

        protected abstract void ReadContent(Element element, IScriptElementMold scriptElementMold);

        protected abstract void ValidateResult(Element element, IScriptElementMold scriptElementMold);

        protected virtual void ReadKeywordValues(Element element, IScriptElementMold scriptElementMold)
        {
            var keywordValues = element.GetAllKeywordArguments();

            foreach (var tuple in keywordValues)
            {
                var keyword = tuple.Item2;
                var keywordValue = tuple.Item3;

                if (keyword.Name.StartsWith(":@"))
                {
                    this.AddProperty(scriptElementMold, keyword.Name, keywordValue);
                }
                else
                {
                    this.ProcessBasicKeyword(scriptElementMold, keyword.Name, keywordValue);
                }
            }
        }

        private void AddProperty(IScriptElementMold scriptElementMold, string keywordName, Element keywordValue)
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

            scriptElementMold.KeywordValues.Add(dictionaryKey, dictionaryValue);
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

        protected internal virtual void ProcessBasicKeyword(
            IScriptElementMold scriptElementMold,
            string keywordName,
            Element keywordValue)
        {
            switch (keywordName)
            {
                case ":NAME":
                    if (keywordValue is StringAtom stringAtom)
                    {
                        scriptElementMold.Name = stringAtom.Value;
                    }
                    else
                    {
                        throw new NotImplementedException("error: script element name must be of type StringAtom.");
                    }
                    break;

                case ":IS-ENTRANCE":
                    if (scriptElementMold is IPartMold partMold1)
                    {
                        partMold1.IsEntrance = keywordValue.ToBool();
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                    break;

                case ":IS-EXIT":
                    if (scriptElementMold is IPartMold partMold2)
                    {
                        partMold2.IsExit = keywordValue.ToBool();
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public IGraphScriptReader ScriptReader { get; }

        public IScriptElementMold Read(IGroupMold owner, Element element)
        {
            var scriptElementMold = this.CreateScriptElementMold(owner, element);

            this.ReadKeywordValues(element, scriptElementMold);
            this.ReadContent(element, scriptElementMold);
            this.ValidateResult(element, scriptElementMold);

            return scriptElementMold;
        }
    }
}
