using System;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.Graphs.Molds.Impl;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading.Impl
{
    public class VertexElementReader : ElementReaderBase
    {
        public VertexElementReader(IGraphScriptReader scriptReader)
            : base(scriptReader)
        {
        }

        protected override IPartMold CreatePartMold(IGroupMold owner)
        {
            if (owner == null)
            {
                throw new NotImplementedException(); // todo: owner of vertex cannot be null
            }

            return new VertexMold(owner);
        }

        protected override void ProcessBasicKeyword(IPartMold partMold, string keywordName, Element keywordValue)
        {
            var vertexMold = (VertexMold)partMold;

            switch (keywordName)
            {
                case ":TYPE":
                    var stringAtom = (StringAtom)keywordValue;
                    vertexMold.Type = stringAtom.Value;
                    break;

                case ":LINKS-TO":
                    var linksTo = PseudoListToStringList(keywordValue);
                    foreach (var linkTo in linksTo)
                    {
                        vertexMold.AddLinkTo(linkTo);
                    }

                    break;

                default:
                    base.ProcessBasicKeyword(partMold, keywordName, keywordValue);
                    break;
            }
        }

        protected override void ReadContent(Element element, IPartMold partMold)
        {
            // idle: vertex does not have content.
        }

        protected override void ValidateResult(Element element, IPartMold partMold)
        {
            // idle.
        }
    }
}
