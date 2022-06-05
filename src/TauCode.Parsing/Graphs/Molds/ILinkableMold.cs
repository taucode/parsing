using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Graphs.Molds
{
    // todo clean, regions
    public interface ILinkableMold : IScriptElementMold
    {
        string GetFullPath();

        bool IsEntrance { get; set; }
        bool IsExit { get; set; }

        IVertexMold GetEntranceVertex();
        IVertexMold GetExitVertex();
    }
}
