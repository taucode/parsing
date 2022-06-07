using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Data.Graphs;
using TauCode.Parsing.Graphs.Molds;

namespace TauCode.Parsing.Graphs.Building
{
    public interface IArcBuilder
    {
        bool Accepts(IArcMold arcMold);
        IArc Build(IArcMold arcMold);
    }
}
