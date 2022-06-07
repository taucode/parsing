using System.Collections.Generic;

namespace TauCode.Parsing.Graphs.Molds
{
    public interface IVertexMold : IPartMold
    {
        string Type { get; set; }

        string FullPath { get; }

        IArcMold AddLinkTo(IVertexMold head);
        IArcMold AddLinkTo(string headPath);

        IArcMold AddLinkFrom(IVertexMold tail);
        IArcMold AddLinkFrom(string tailPath);

        IReadOnlyList<IArcMold> OutgoingArcs { get; }
        IReadOnlyList<IArcMold> IncomingArcs { get; }
    }
}
