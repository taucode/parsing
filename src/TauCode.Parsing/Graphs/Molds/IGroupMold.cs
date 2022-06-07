using System.Collections.Generic;

namespace TauCode.Parsing.Graphs.Molds
{
    public interface IGroupMold : IPartMold
    {
        string FullPath { get; }

        IReadOnlyList<IPartMold> Content { get; }

        void Add(IPartMold part);
    }
}
