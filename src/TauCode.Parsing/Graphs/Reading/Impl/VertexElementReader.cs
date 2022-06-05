using System;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.Graphs.Molds.Impl;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading.Impl
{
    public class VertexElementReader : ScriptElementReaderBase
    {
        public VertexElementReader(IGraphScriptReader scriptReader)
            : base(scriptReader)
        {
        }

        protected override IScriptElementMold CreateScriptElementMold(IGroupMold owner, Element element)
        {
            if (owner == null)
            {
                throw new NotImplementedException(); // todo: owner of vertex cannot be null
            }

            return new VertexMold(owner, (Atom)element.GetCar());
        }

        //protected internal override void ProcessBasicKeyword(
        //    IScriptElementMold scriptElementMold,
        //    string keywordName,
        //    Element keywordValue)
        //{
        //    var vertexMold = (VertexMold)scriptElementMold;

        //    switch (keywordName)
        //    {
        //        case ":TYPE":
        //            var stringAtom = (StringAtom)keywordValue; // todo can throw
        //            vertexMold.Type = stringAtom.Value;
        //            break;

        //        case ":LINKS-TO":
        //            var linksTo = PseudoListToStringList(keywordValue); // todo can throw
        //            foreach (var linkTo in linksTo)
        //            {
        //                vertexMold.AddLinkTo(linkTo);
        //            }

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
