using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Data.Graphs;
using TauCode.Parsing.Graphs.Molding;

namespace TauCode.Parsing.Graphs.Building
{
    public interface IArcFactory
    {
        IArc Create(IArcMold arcMold);
    }
}
