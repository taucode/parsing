using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Graphs.Molding;
using TauCode.Parsing.Graphs.Molding.Impl;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading.Impl;

public class SequenceReader : GroupReader
{
    public SequenceReader(IGraphScriptReader scriptReader)
        : base(scriptReader)
    {
    }

    protected override void ReadContent(IScriptElementMold scriptElementMold, Element element)
    {
        base.ReadContent(scriptElementMold, element);

        var groupMold = (GroupMold)scriptElementMold;
        var zeroth = groupMold.Linkables[0];
        var last = groupMold.Linkables[^1]; // todo: can throw here if empty seq.

        zeroth.IsEntrance = true;
        last.IsExit = true;
    }

    protected override void CustomizeContent(IScriptElementMold scriptElementMold, Element element)
    {
        var group = (GroupMold)scriptElementMold;

        var linkables = group.Linkables;
        if (linkables.Count == 0)
        {
            throw new GraphException("Empty sequence.");
        }

        for (var i = 0; i < linkables.Count; i++)
        {
            var linkableMold = linkables[i];

            if (i < linkables.Count - 1)
            {
                var nextLinkableMold = linkables[i + 1];
                linkableMold.AddLinkTo(nextLinkableMold);
            }
        }
    }
}