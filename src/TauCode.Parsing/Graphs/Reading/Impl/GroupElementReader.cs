using System;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.Graphs.Molds.Impl;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading.Impl
{
    // todo clean, regions
    public class GroupElementReader : ScriptElementReaderBase
    {
        public GroupElementReader(IGraphScriptReader scriptReader)
            : base(scriptReader)
        {
        }

        protected override IScriptElementMold CreateScriptElementMold(IGroupMold owner)
        {
            IScriptElementMold scriptElementMold = new GroupMold(owner);
            return scriptElementMold;
        }

        //protected override IPartMold CreatePartMold(IGroupMold owner)
        //{
        //    IPartMold result = new GroupMold(owner);
        //    return result;
        //}

        protected override void ReadContent(Element element, IScriptElementMold scriptElementMold)
        {
            var pseudoList = (PseudoList)element; // todo: can throw?

            var content = pseudoList.GetFreeArguments();
            var groupMold = (IGroupMold)scriptElementMold; // todo: can throw?

            foreach (var contentElement in content)
            {
                var contentPseudoList = (PseudoList)contentElement; // todo: can throw?
                var car = contentPseudoList[0];

                if (car is Atom contentElementCar)
                {
                    var innerElementReader = this.ScriptReader.ResolveElementReader(contentElementCar);
                    var innerPartMold = innerElementReader.Read(groupMold, contentElement);
                    groupMold.Add(innerPartMold);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        protected override void ValidateResult(Element element, IScriptElementMold scriptElementMold)
        {
            // todo: idle?
        }
    }
}
