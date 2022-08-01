using TauCode.Parsing.Graphs.Molding;
using TauCode.Parsing.Graphs.Molding.Impl;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Reading.Impl;

public class GroupReader : ScriptElementReaderBase
{
    #region ctor

    public GroupReader(IGraphScriptReader scriptReader)
        : base(scriptReader)
    {
    }


    #endregion

    #region Overridden

    protected override IScriptElementMold CreateScriptElementMold(IGroupMold? owner, Element element)
    {
        // todo cast can throw
        IScriptElementMold scriptElementMold = new GroupMold(owner, (Atom)element.GetCar());
        return scriptElementMold;
    }

    protected override void ReadContent(IScriptElementMold scriptElementMold, Element element)
    {
        // many casts, all can throw. todo.
        var pseudoList = (PseudoList)element;

        var content = pseudoList.GetFreeArguments();
        var groupMold = (GroupMold)scriptElementMold;

        foreach (var contentElement in content)
        {
            var contentPseudoList = (PseudoList)contentElement;
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

    #endregion
}