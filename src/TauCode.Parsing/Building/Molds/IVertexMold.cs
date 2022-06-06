using System.Collections.Generic;

namespace TauCode.Parsing.Building.Molds
{
    public interface IVertexMold : IGraphPartMold
    {
        IGroupMold Owner { get; }

        string VertexName { get; set; }

        string GetFullVertexName();

        IArcMold AddLinkTo(IVertexMold head);
        IArcMold AddLinkTo(string headName);

        IArcMold AddLinkFrom(IVertexMold tail);
        IArcMold AddLinkFrom(string tailName);

        IReadOnlyList<IArcMold> OutgoingArcs { get; }
        IReadOnlyList<IArcMold> IncomingArcs { get; }
    }
}
