using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molding.Impl
{
    public abstract class LinkableMoldBase : ScriptElementMoldBase, ILinkableMold
    {
        #region Fields

        private bool _isEntrance;
        private bool _isExit;

        #endregion

        #region ctor

        protected LinkableMoldBase(IGroupMold owner, Atom car)
            : base(owner, car)
        {   
        }

        #endregion

        #region Protected

        protected abstract IVertexMold GetEntranceVertexImpl();

        protected abstract IVertexMold GetExitVertexImpl();

        #endregion

        #region Overridden

        public override void ProcessKeywords()
        {
            base.ProcessKeywords();

            this.IsEntrance = this.GetKeywordValueAsBool(":IS-ENTRANCE");
            this.IsExit = this.GetKeywordValueAsBool(":IS-EXIT");
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

        public IVertexMold GetExitVertex()
        {
            this.CheckFinalized();

            return this.GetExitVertexImpl();
        }
        
        #endregion
    }
}
