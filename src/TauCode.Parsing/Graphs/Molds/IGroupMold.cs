using System.Collections.Generic;

namespace TauCode.Parsing.Graphs.Molds
{
    public interface IGroupMold : ILinkableMold
    {
        IReadOnlyList<IScriptElementMold> AllElements { get; }
        IReadOnlyList<ILinkableMold> Linkables { get; }

        void Add(IScriptElementMold scriptElementMold);
    }
}
