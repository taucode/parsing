using System;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.Graphs.Molds.Impl;
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

            // todo: add tool-generated name
        };

        var optionalExit = new VertexMold(groupMold, Symbol.Create("idle"))
        {
            IsExit = true,
            // todo: add tool-generated name
        };

        optionalEntrance.AddLinkTo(linkable.GetEntranceVertex());
        optionalEntrance.AddLinkTo(optionalExit);

        linkable.GetExitVertex().AddLinkTo(optionalExit);

        optionalEntrance.ValidateAndFinalize();
        optionalExit.ValidateAndFinalize();

        groupMold.Add(optionalEntrance);
        groupMold.Add(optionalExit);
    }

    //protected override void ReadContent(Element element, IScriptElementMold scriptElementMold)
    //{
    //    var groupMold = (GroupMold)scriptElementMold;

    //    base.ReadContent(element, groupMold);

    //    PartMoldBase innerPartMold = null;

    //    foreach (var innerScriptElementMold in groupMold.AllElements)
    //    {
    //        if (innerScriptElementMold is PartMoldBase foundInnerPartMold)
    //        {
    //            if (innerPartMold != null)
    //            {
    //                throw new NotImplementedException("error: more than one inner part");
    //            }

    //            innerPartMold = foundInnerPartMold;
    //        }
    //    }

    //    if (innerPartMold == null)
    //    {
    //        throw new NotImplementedException("error: no inner part");
    //    }

    //    if (innerPartMold.IsEntrance || innerPartMold.IsExit)
    //    {
    //        throw new NotImplementedException("error: isEntrance and isExit not applicable here.");
    //    }

    //    if (innerPartMold.Entrance == null)
    //    {
    //        throw new NotImplementedException("error: optional content must have entrance.");
    //    }

    //    if (innerPartMold.Exit == null)
    //    {
    //        throw new NotImplementedException("error: optional content must have exit.");
    //    }

    //    var optionalEntrance = new VertexMold(groupMold, Symbol.Create("idle"));
    //    var optionalExit = new VertexMold(groupMold, Symbol.Create("idle"));

    //    optionalEntrance.AddLinkTo(innerPartMold.Entrance);
    //    optionalEntrance.AddLinkTo(optionalExit);

    //    innerPartMold.Exit.AddLinkTo(optionalExit);

    //    groupMold.Entrance = optionalEntrance;
    //    groupMold.Exit = optionalExit;
    //}
}
