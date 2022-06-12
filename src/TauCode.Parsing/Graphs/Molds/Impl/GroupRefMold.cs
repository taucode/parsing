using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    // todo regions
    public class GroupRefMold : LinkableMoldBase, IGroupRefMold
    {
        private string _referencedGroupPath;
        private readonly GroupRefEntranceVertexResolver _entrance;
        private readonly GroupRefExitVertexResolver _exit;

        public GroupRefMold(IGroupMold owner)
            : base(owner, Symbol.Create("GROUP-REF"))
        {
            _entrance = new GroupRefEntranceVertexResolver(this);
            _exit = new GroupRefExitVertexResolver(this);
        }

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

        //public override IVertexMold Entrance
        //{
        //    get => _entrance;
        //    set => throw new NotImplementedException("error: invalid operation");
        //}

        //public override IVertexMold Exit
        //{
        //    get => _exit;
        //    set => throw new NotImplementedException("error: invalid operation");
        //}

        //public string ReferencedGroupPath { get; set; }
        protected override void ValidateAndFinalizeImpl()
        {
            if (this.ReferencedGroupPath == null)
            {
                throw new NotImplementedException("error: should be set.");
            }
        }

        public string ReferencedGroupPath
        {
            get => _referencedGroupPath;
            set
            {
                this.CheckNotFinalized();
                _referencedGroupPath = value;
            }
        }
    }
}
