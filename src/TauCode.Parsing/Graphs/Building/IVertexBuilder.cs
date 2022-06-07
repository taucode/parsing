using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Data.Graphs;
using TauCode.Parsing.Graphs.Molds;

namespace TauCode.Parsing.Graphs.Building
{
    public interface IVertexBuilder
    {
        bool Accepts(IVertexMold vertexMold);
        IVertex Build(IVertexMold vertexMold);
    }
}
