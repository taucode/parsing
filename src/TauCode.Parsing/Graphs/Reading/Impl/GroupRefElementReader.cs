using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.Graphs.Molds.Impl;
using TauCode.Parsing.TinyLisp.Data;

// todo regions, clean
namespace TauCode.Parsing.Graphs.Reading.Impl
{
    public class GroupRefElementReader : ScriptElementReaderBase
    {
        private readonly VertexElementReader _vertexElementReader;

        public GroupRefElementReader(IGraphScriptReader scriptReader)
            : base(scriptReader)
        {
            _vertexElementReader = new VertexElementReader(scriptReader);
        }

        protected override IScriptElementMold CreateScriptElementMold(IGroupMold owner, Element element)
        {
            IScriptElementMold scriptElementMold = new GroupRefMold(owner);
            return scriptElementMold;
        }

        //protected internal override void ProcessBasicKeyword(
        //    IScriptElementMold scriptElementMold,
        //    string keywordName,
        //    Element keywordValue)
        //{
        //    var groupRefMold = (GroupRefMold)scriptElementMold;

        //    switch (keywordName)
        //    {
        //        case ":GROUP-PATH":
        //            var stringAtom = (StringAtom)keywordValue; // todo can throw
        //            groupRefMold.ReferencedGroupPath = stringAtom.Value;
        //            break;

        //        case ":LINKS-TO":
        //            _vertexElementReader.ProcessBasicKeyword(
        //                groupRefMold.Exit,
        //                keywordName,
        //                keywordValue);

        //            //var linksTo = PseudoListToStringList(keywordValue);
        //            //foreach (var linkTo in linksTo)
        //            //{
        //            //    vertexMold.AddLinkTo(linkTo);
        //            //}

        //            break;

        //        default:
        //            base.ProcessBasicKeyword(scriptElementMold, keywordName, keywordValue);
        //            break;
        //    }
        //}

        


        protected override void ReadContent(IScriptElementMold scriptElementMold, Element element)
        {
            // idle. todo: check that is no content
        }

        protected override void CustomizeContent(IScriptElementMold scriptElementMold, Element element)
        {
            // idle.
        }

        //protected override void FinalizeMold(IScriptElementMold scriptElementMold, Element element)
        //{
        //    // idle.
        //}
    }
}
