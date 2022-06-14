using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Graphs.Molding;
using TauCode.Parsing.Graphs.Molding.Impl;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading.Impl
{
    public abstract class ScriptElementReaderBase : IScriptElementReader
    {
        #region ctor

        protected ScriptElementReaderBase(IGraphScriptReader scriptReader)
        {
            this.ScriptReader = scriptReader ?? throw new ArgumentNullException(nameof(scriptReader));
        }

        #endregion

        #region IScriptElementReader Members

        public IGraphScriptReader ScriptReader { get; }

        public IScriptElementMold Read(IGroupMold owner, Element element)
        {
            var scriptElementMold = this.CreateScriptElementMold(owner, element);

            ((ScriptElementMoldBase)scriptElementMold).LispElement = element;

            this.ReadKeywordValues(scriptElementMold, element);
            scriptElementMold.ProcessKeywords();
            this.ReadContent(scriptElementMold, element);
            this.CustomizeContent(scriptElementMold, element);

            scriptElementMold.ValidateAndFinalize();

            return scriptElementMold;
        }

        #endregion

        #region Protected

        protected abstract IScriptElementMold CreateScriptElementMold(IGroupMold owner, Element element);

        protected abstract void CustomizeContent(IScriptElementMold scriptElementMold, Element element);

        protected abstract void ReadContent(IScriptElementMold scriptElementMold, Element element);

        protected virtual void ReadKeywordValues(IScriptElementMold scriptElementMold, Element element)
        {
            var keywordValues = element.GetAllKeywordArguments();

            foreach (var (_, keyword, keywordValue) in keywordValues)
            {
                AddKeywordValue(scriptElementMold, keyword.Name, keywordValue);
            }
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

        #endregion

        #region Private

        private static void AddKeywordValue(IScriptElementMold scriptElementMold, string keyword, Element keywordValue)
        {
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

            scriptElementMold.SetKeywordValue(keyword, dictionaryValue);
        }

        #endregion
    }
}
