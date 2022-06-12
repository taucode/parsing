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

        ///// <summary>
        ///// When true, this part is the entrance for its owner group.
        ///// If this property is true, then <see cref="Entrance"/> must not be null.
        ///// </summary>
        //bool IsEntrance { get; set; }

        ///// <summary>
        ///// When true, this part is the exit for its owner group.
        ///// If this property is true, then <see cref="Exit"/> must not be null.
        ///// </summary>
        //bool IsExit { get; set; }

        //IVertexMold Entrance { get; }
        //IVertexMold Exit { get; }
    }
}
