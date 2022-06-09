using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    public abstract class PartMoldBase : ScriptElementMoldBase, IPartMold
    {
        #region ctor

        protected PartMoldBase(IGroupMold owner)
            : base(owner)
        {   
        }

        #endregion

        #region IGraphPartMold Members

        public abstract string GetFullPath();

        public bool IsEntrance { get; set; }
        public bool IsExit { get; set; }
        public abstract IVertexMold Entrance { get; internal set; }
        public abstract IVertexMold Exit { get; internal set; }

        #endregion
    }
}
