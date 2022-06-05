using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Data.Graphs;
using TauCode.Parsing.Graphs.Molds;

namespace TauCode.Parsing.Graphs.Building
{
    // todo clean
    public interface IVertexFactory
    {
        //bool Accepts(IVertexMold vertexMold);
        IVertex Create(IVertexMold vertexMold);
    }
}
