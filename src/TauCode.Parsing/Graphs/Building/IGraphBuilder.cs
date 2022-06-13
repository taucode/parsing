﻿using System.Collections.Generic;
using TauCode.Data.Graphs;
using TauCode.Parsing.Graphs.Molds;

namespace TauCode.Parsing.Graphs.Building
{
    public interface IGraphBuilder
    {
        IGraph Build(IGroupMold group);
    }
}