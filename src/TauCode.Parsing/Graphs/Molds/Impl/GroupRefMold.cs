using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    // todo regions
    public class GroupRefMold : PartMoldBase, IGroupRefMold
    {
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

        public override IVertexMold Entrance
        {
            get => _entrance;
            set => throw new NotImplementedException("error: invalid operation");
        }

        public override IVertexMold Exit
        {
            get => _exit;
            set => throw new NotImplementedException("error: invalid operation");
        }

        public string ReferencedGroupPath { get; set; }
    }
}
