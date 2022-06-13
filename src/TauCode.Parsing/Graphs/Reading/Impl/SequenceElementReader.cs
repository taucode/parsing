using System;
using TauCode.Parsing.Graphs.Molding;
using TauCode.Parsing.Graphs.Molding.Impl;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading.Impl
{
    public class SequenceElementReader : GroupElementReader
    {
        public SequenceElementReader(IGraphScriptReader scriptReader)
            : base(scriptReader)
        {
        }

        protected override void ReadContent(IScriptElementMold scriptElementMold, Element element)
        {
            base.ReadContent(scriptElementMold, element);

            var groupMold = (GroupMold)scriptElementMold;
            var zeroth = groupMold.Linkables[0];
            var last = groupMold.Linkables[^1];

            if (!zeroth.IsEntrance)
            {
                throw new NotImplementedException("error: 0th element of sequence must be entrance.");
            }

            if (!last.IsExit)
            {
                throw new NotImplementedException("error: last element of sequence must be exit.");
            }
        }

        protected override void CustomizeContent(IScriptElementMold scriptElementMold, Element element)
        {
            var group = (GroupMold)scriptElementMold;

            var linkables = group.Linkables;
            if (linkables.Count == 0)
            {
                throw new NotImplementedException("empty list");
            }

            for (var i = 0; i < linkables.Count; i++)
            {
                var linkableMold = linkables[i];

                if (i < linkables.Count - 1)
                {
                    var nextLinkableMold = linkables[i + 1];
                    linkableMold.GetExitVertexOrThrow().AddLinkTo(nextLinkableMold.GetEntranceVertexOrThrow());
                }
            }
        }
    }
}
