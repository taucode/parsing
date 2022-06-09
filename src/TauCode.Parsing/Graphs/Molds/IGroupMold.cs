using System.Collections.Generic;

namespace TauCode.Parsing.Graphs.Molds
{
    public interface IGroupMold : IPartMold
    {
        IReadOnlyList<IScriptElementMold> Content { get; }

        void Add(IScriptElementMold scriptElementMold);
    }
}
