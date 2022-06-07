using System.Collections.Generic;

namespace TauCode.Parsing.Graphs.Molds
{
    public interface IGroupMold : IPartMold
    {
        /// <summary>
        /// Name of the group
        /// </summary>
        string Name { get; set; }

        string FullPath { get; }

        IReadOnlyList<IPartMold> Content { get; }

        void Add(IPartMold part);
    }
}
