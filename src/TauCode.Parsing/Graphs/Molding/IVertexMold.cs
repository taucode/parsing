using System.Collections.Generic;

namespace TauCode.Parsing.Graphs.Molding
{
    public interface IVertexMold : ILinkableMold
    {
        string TypeAlias { get; set; }

        IArcMold AddLinkTo(IVertexMold head);
        IArcMold AddLinkTo(string headPath);

        IArcMold AddLinkFrom(IVertexMold tail);
        IArcMold AddLinkFrom(string tailPath);

        IReadOnlyList<IArcMold> OutgoingArcs { get; }
        IReadOnlyList<IArcMold> IncomingArcs { get; }
    }
}
