using System;
using TauCode.Parsing.Graphs.Molding;
using TauCode.Parsing.Graphs.Molding.Impl;
using TauCode.Parsing.Graphs.Reading;
using TauCode.Parsing.Graphs.Reading.Impl;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Tests.Parsing.Sql.ScriptElementReaders;

public class OptionalElementReader : GroupElementReader
{
    public OptionalElementReader(IGraphScriptReader scriptReader)
        : base(scriptReader)
    {
    }

    protected override void CustomizeContent(IScriptElementMold scriptElementMold, Element element)
    {
        var groupMold = (GroupMold)scriptElementMold;

        var linkables = groupMold.Linkables;
        if (linkables.Count != 1)
        {
            throw new NotImplementedException("error: optional wants exactly one child");
        }

        var linkable = linkables[0];
        var optionalEntrance = new VertexMold(groupMold, Symbol.Create("idle"))
        {
            IsEntrance = true,
        };

        var optionalExit = new VertexMold(groupMold, Symbol.Create("idle"))
        {
            IsExit = true,
        };

        optionalEntrance.AddLinkTo(linkable.GetEntranceVertex());
        optionalEntrance.AddLinkTo(optionalExit);

        linkable.GetExitVertex().AddLinkTo(optionalExit);

        optionalEntrance.ValidateAndFinalize();
        optionalExit.ValidateAndFinalize();

        groupMold.Add(optionalEntrance);
        groupMold.Add(optionalExit);
    }
}
