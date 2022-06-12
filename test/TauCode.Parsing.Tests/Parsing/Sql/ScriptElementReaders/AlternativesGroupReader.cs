using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TauCode.Parsing.Graphs.Molds;
using TauCode.Parsing.Graphs.Molds.Impl;
using TauCode.Parsing.Graphs.Reading;
using TauCode.Parsing.Graphs.Reading.Impl;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Tests.Parsing.Sql.ScriptElementReaders;

public class AlternativesGroupReader : GroupElementReader
{
    public AlternativesGroupReader(IGraphScriptReader scriptReader)
        : base(scriptReader)
    {
    }

    protected override void CustomizeContent(IScriptElementMold scriptElementMold, Element element)
    {
        var alternativesGroupMold = (GroupMold)scriptElementMold;

        var entrance = new VertexMold(alternativesGroupMold, Symbol.Create("idle"))
        {
            IsEntrance = true,
        };
        entrance.ValidateAndFinalize();
        alternativesGroupMold.Add(entrance);

        var exit = new VertexMold(alternativesGroupMold, Symbol.Create("idle"))
        {
            IsExit = true,
        };
        exit.ValidateAndFinalize();
        alternativesGroupMold.Add(exit);

        for (var i = 1; i < alternativesGroupMold.Linkables.Count - 2; i++)
        {
            var innerLinkable = alternativesGroupMold.Linkables[i];
            if (innerLinkable.GetEntranceVertex() == null)
            {
                throw new NotImplementedException("error: in alternatives, all elements must have entrance.");
            }

            entrance.AddLinkTo(innerLinkable.GetEntranceVertex());

            if (innerLinkable.GetExitVertex() == null)
            {
                throw new NotImplementedException("error: in alternatives, all elements must have exit.");
            }

            innerLinkable.GetExitVertex().AddLinkTo(exit);
        }

        //alternativesGroupMold.Entrance = entrance;
        //alternativesGroupMold.Exit = exit;

    }

    //protected override void ReadContent(Element element, IScriptElementMold scriptElementMold)
    //{
    //    var alternativesGroupMold = (GroupMold)scriptElementMold;

    //    var entrance = new VertexMold(alternativesGroupMold, Symbol.Create("idle"));
    //    alternativesGroupMold.Add(entrance);

    //    base.ReadContent(element, alternativesGroupMold);

    //    var exit = new VertexMold(alternativesGroupMold, Symbol.Create("idle"));
    //    alternativesGroupMold.Add(exit);

    //    for (var i = 1; i < alternativesGroupMold.AllElements.Count - 2; i++)
    //    {
    //        var innerScriptElement = alternativesGroupMold.AllElements[i];
    //        if (innerScriptElement is IPartMold innerPartMold)
    //        {
    //            if (innerPartMold.Entrance == null)
    //            {
    //                throw new NotImplementedException("error: in alternatives, all elements must have entrance.");
    //            }

    //            entrance.AddLinkTo(innerPartMold.Entrance);

    //            if (innerPartMold.Exit == null)
    //            {
    //                throw new NotImplementedException("error: in alternatives, all elements must have exit.");
    //            }

    //            innerPartMold.Exit.AddLinkTo(exit);
    //        }
    //    }

    //    alternativesGroupMold.Entrance = entrance;
    //    alternativesGroupMold.Exit = exit;
    //}
}
