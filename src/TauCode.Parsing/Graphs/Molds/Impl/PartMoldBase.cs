using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    public abstract class PartMoldBase : IPartMold
    {
        #region ctor

        protected PartMoldBase(IGroupMold owner)
        {
            this.Owner = owner;
        }

        #endregion

        #region IGraphPartMold Members

        public IGroupMold Owner { get; }
        public string Name { get; set; }
        public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();
        public bool IsEntrance { get; set; }
        public bool IsExit { get; set; }
        public abstract IVertexMold Entrance { get; }
        public abstract IVertexMold Exit { get; }

        #endregion
    }
}
