using System;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molding.Impl
{
    public class GroupRefMold : LinkableMoldBase, IGroupRefMold
    {
        #region Fields

        private string _referencedGroupPath;
        private readonly GroupRefEntranceVertexResolver _entrance;
        private readonly GroupRefExitVertexResolver _exit;

        #endregion

        #region ctor

        public GroupRefMold(IGroupMold owner)
            : base(owner, Symbol.Create("GROUP-REF"))
        {
            _entrance = new GroupRefEntranceVertexResolver(this);
            _exit = new GroupRefExitVertexResolver(this);
        }

        #endregion

        #region IGroupRefMold Members

        public string ReferencedGroupPath
        {
            get => _referencedGroupPath;
            set
            {
                this.CheckNotFinalized();
                _referencedGroupPath = value;
            }
        }

        #endregion

        #region Overridden

        public override string GetFullPath()
        {
            throw new NotImplementedException();
        }

        protected override IVertexMold GetEntranceVertexImpl() => _entrance;

        protected override IVertexMold GetExitVertexImpl() => _exit;

        public override void ProcessKeywords()
        {
            base.ProcessKeywords();

            _referencedGroupPath = (string)this.GetKeywordValue(":GROUP-PATH");
        }

        protected override void ValidateAndFinalizeImpl()
        {
            if (this.ReferencedGroupPath == null)
            {
                throw new NotImplementedException("error: should be set.");
            }
        }


        #endregion
    }
}
