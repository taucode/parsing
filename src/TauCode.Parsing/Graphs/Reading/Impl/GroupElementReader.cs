using System;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.Graphs.Molds.Impl;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading.Impl
{
    public class GroupElementReader : ElementReaderBase
    {
        public GroupElementReader(IGraphScriptReader scriptReader)
            : base(scriptReader)
        {
        }

        protected override IPartMold CreatePartMold(IGroupMold owner)
        {
            IPartMold result = new GroupMold(owner);
            return result;
        }

        protected override void ReadContent(Element element, IPartMold partMold)
        {
            var pseudoList = (PseudoList)element; // todo: can throw?

            var content = pseudoList.GetFreeArguments();
            var groupMold = (IGroupMold)partMold; // todo: can throw?

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

        protected override void ValidateResult(Element element, IPartMold partMold)
        {
            // todo: idle?
        }
    }
}
