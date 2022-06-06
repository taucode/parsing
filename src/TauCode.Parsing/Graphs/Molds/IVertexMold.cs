﻿using System.Collections.Generic;

namespace TauCode.Parsing.Graphs.Molds
{
    public interface IVertexMold : IInsertableMold
    {
        string Name { get; set; }

        string FullPath { get; }

        IArcMold AddLinkTo(IVertexMold head);
        IArcMold AddLinkTo(string headFullPath);

        IArcMold AddLinkFrom(IVertexMold tail);
        IArcMold AddLinkFrom(string tailFullPath);

        IReadOnlyList<IArcMold> OutgoingArcs { get; }
        IReadOnlyList<IArcMold> IncomingArcs { get; }
    }
}
