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
            IReadOnlyDictionary<string, string> initialProperties)
            : base(scriptReader)
        {
            // todo checks
            this.InitialProperties = initialProperties;
        }

        public IReadOnlyDictionary<string, string> InitialProperties { get; }

        protected override IScriptElementMold CreateScriptElementMold(IGroupMold owner)
        {
            var scriptElementMold = base.CreateScriptElementMold(owner);
            foreach (var pair in this.InitialProperties)
            {
                scriptElementMold.Properties.Add(pair.Key, pair.Value);
            }

            return scriptElementMold;
        }

        // todo: protected internal - not very good. better consider public (or internal) static methods extracting things like ":LINKS-TO" etc.
        protected internal override void ProcessBasicKeyword(
            IScriptElementMold scriptElementMold,
            string keywordName,
            Element keywordValue)
        {
            if (this.InitialProperties.ContainsKey(keywordName))
            {
                throw new NotImplementedException($"error: '{keywordName}' is contained in initial props.");
            }

            base.ProcessBasicKeyword(scriptElementMold, keywordName, keywordValue);
        }
    }
}
