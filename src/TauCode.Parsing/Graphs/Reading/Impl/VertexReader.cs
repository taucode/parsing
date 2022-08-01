using TauCode.Parsing.Graphs.Molding;
using TauCode.Parsing.Graphs.Molding.Impl;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading.Impl;

public class VertexReader : ScriptElementReaderBase
{
    #region ctor

    public VertexReader(IGraphScriptReader scriptReader)
        : base(scriptReader)
    {
    }

    #endregion

    #region Overridden

    protected override IScriptElementMold CreateScriptElementMold(IGroupMold? owner, Element element)
    {
        if (owner == null)
        {
            throw new ArgumentNullException(nameof(owner));
        }

        return new VertexMold(owner, (Atom)element.GetCar()); // todo: can throw? here and anywhere
    }

    protected override void ReadContent(IScriptElementMold scriptElementMold, Element element)
    {
        // idle.
    }

    protected override void CustomizeContent(IScriptElementMold scriptElementMold, Element element)
    {
        // idle.
    }

    #endregion
}