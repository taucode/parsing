using System;
using System.Collections.Generic;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading.Impl
{
    public class CustomVertexElementReader : VertexElementReader
    {
        public CustomVertexElementReader(
            IGraphScriptReader scriptReader,
            IReadOnlyDictionary<string, string> initialKeywordValues)
            : base(scriptReader)
        {
            // todo checks
            this.InitialKeywordValues = initialKeywordValues;
        }

        public IReadOnlyDictionary<string, string> InitialKeywordValues { get; }

        //protected override IScriptElementMold CreateScriptElementMold(IGroupMold owner, Element element)
        //{
        //    var scriptElementMold = base.CreateScriptElementMold(owner, element);
        //    foreach (var pair in this.InitialKeywordValues)
        //    {
        //        scriptElementMold.KeywordValues.Add(pair.Key, pair.Value);
        //    }

        //    return scriptElementMold;
        //}

        // todo: protected internal - not very good. better consider public (or internal) static methods extracting things like ":LINKS-TO" etc.
        //protected internal override void ProcessBasicKeyword(
        //    IScriptElementMold scriptElementMold,
        //    string keywordName,
        //    Element keywordValue)
        //{
        //    if (this.InitialKeywordValues.ContainsKey(keywordName))
        //    {
        //        throw new NotImplementedException($"error: '{keywordName}' is contained in initial props.");
        //    }

        //    base.ProcessBasicKeyword(scriptElementMold, keywordName, keywordValue);
        //}
    }
}
