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
        public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();
        public virtual IVertexMold Entrance => throw new NotImplementedException();
        public virtual IVertexMold Exit => throw new NotImplementedException();

        #endregion
    }
}
