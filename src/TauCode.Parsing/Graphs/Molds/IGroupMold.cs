using System.Collections.Generic;

namespace TauCode.Parsing.Graphs.Molds
{
    public interface IGroupMold : IInsertableMold
    {
        /// <summary>
        /// Name of the group
        /// </summary>
        string Name { get; }

        string FullPath { get; }

        ISet<IGraphPartMold> Content { get; }

        void Add(IGraphPartMold part);
    }
}
