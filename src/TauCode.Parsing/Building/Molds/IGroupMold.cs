using System.Collections.Generic;

namespace TauCode.Parsing.Building.Molds
{
    public interface IGroupMold : IGraphPartMold
    {
        string GroupName { get; }

        string GetFullGroupName();

        ISet<IGraphPartMold> Content { get; }

        IVertexMold Entrance { get; set; }

        IVertexMold Exit { get; set; }

        void AddGroup(IGroupMold group);

        void AddVertex(IVertexMold vertex);

        void AddArc(IArcMold arc);
    }
}
