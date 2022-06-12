using System;
using System.Collections.Generic;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    // todo clean
    public abstract class LinkableMoldBase : ScriptElementMoldBase, ILinkableMold
    {
        private bool _isEntrance;
        private bool _isExit;

        #region ctor

        protected LinkableMoldBase(IGroupMold owner, Atom car)
            : base(owner, car)
        {   
        }

        #endregion

        #region ILinkableMold Members

        public abstract string GetFullPath();

        public bool IsEntrance
        {
            get => _isEntrance;
            set
            {
                this.CheckNotFinalized();
                _isEntrance = value;
            }
        }

        public bool IsExit
        {
            get => _isExit;
            set
            {
                this.CheckNotFinalized();
                _isExit = value;
            }
        }

        public IVertexMold GetEntranceVertex()
        {
            this.CheckFinalized();

            return this.GetEntranceVertexImpl();
        }

        protected abstract IVertexMold GetEntranceVertexImpl();

        public IVertexMold GetExitVertex()
        {
            this.CheckFinalized();

            return this.GetExitVertexImpl();
        }

        protected abstract IVertexMold GetExitVertexImpl();

        public override void ProcessKeywords()
        {
            base.ProcessKeywords();

            this.IsEntrance = this.GetKeywordValueAsBool(":IS-ENTRANCE");
            this.IsExit = this.GetKeywordValueAsBool(":IS-EXIT");
        }

        //public bool IsEntrance { get; set; }
        //public bool IsExit { get; set; }
        //public abstract IVertexMold Entrance { get; set; }
        //public abstract IVertexMold Exit { get; set; }

        #endregion
    }
}
