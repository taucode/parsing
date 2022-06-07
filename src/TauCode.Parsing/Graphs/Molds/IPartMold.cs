using System.Collections.Generic;

namespace TauCode.Parsing.Graphs.Molds
{
    public interface IPartMold
    {
        IGroupMold Owner { get; }
        string Name { get; set; }
        IDictionary<string, object> Properties { get; }

        /// <summary>
        /// When true, this part is the entrance for its owner group.
        /// If this property is true, then <see cref="Entrance"/> must not be null.
        /// </summary>
        bool IsEntrance { get; set; }

        /// <summary>
        /// When true, this part is the exit for its owner group.
        /// If this property is true, then <see cref="Exit"/> must not be null.
        /// </summary>
        bool IsExit { get; set; }

        IVertexMold Entrance { get; }
        IVertexMold Exit { get; }
    }
}
