﻿using System;
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

        protected override IScriptElementMold CreateScriptElementMold(IGroupMold owner, Element element)
        {
            IScriptElementMold scriptElementMold = new GroupMold(owner, (Atom)element.GetCar()); // todo can throw
            return scriptElementMold;
        }

        protected override void ReadContent(IScriptElementMold scriptElementMold, Element element)
        {
            var pseudoList = (PseudoList)element; // todo: can throw?

            var content = pseudoList.GetFreeArguments();
            var groupMold = (GroupMold)scriptElementMold; // todo: can throw?

            foreach (var contentElement in content)
            {
                var contentPseudoList = (PseudoList)contentElement; // todo: can throw?
                var car = contentPseudoList[0];

                if (car is Atom contentElementCar)
                {
                    var innerElementReader = this.ScriptReader.ResolveElementReader(contentElementCar);
                    var innerScriptElementMold = innerElementReader.Read(groupMold, contentElement);

                    groupMold.Add(innerScriptElementMold);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        protected override void CustomizeContent(IScriptElementMold scriptElementMold, Element element)
        {
            // idle
        }
    }
}