using TauCode.Parsing.Graphs.Molding;
using TauCode.Parsing.Graphs.Molding.Impl;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading.Impl;

public class RefReader : ScriptElementReaderBase
{
    public RefReader(IGraphScriptReader scriptReader)
        : base(scriptReader)
    {
    }

    protected override IScriptElementMold CreateScriptElementMold(IGroupMold? owner, Element element)
    {
        return new RefMold(owner!, (Atom)element.GetCar());
    }

    protected override void CustomizeContent(IScriptElementMold scriptElementMold, Element element)
    {
        // idle.
    }

    protected override void ReadContent(IScriptElementMold scriptElementMold, Element element)
    {
        // idle.
    }
}