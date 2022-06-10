using System;
using System.Collections.Generic;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    public abstract class PartMoldBase : ScriptElementMoldBase, IPartMold
    {
        #region ctor

        protected PartMoldBase(IGroupMold owner, Atom car)
            : base(owner, car)
        {   
        }

        #endregion

        #region IGraphPartMold Members

        public abstract string GetFullPath();

        public bool IsEntrance { get; set; }
        public bool IsExit { get; set; }
        public abstract IVertexMold Entrance { get; set; }
        public abstract IVertexMold Exit { get; set; }

        #endregion
    }
}
