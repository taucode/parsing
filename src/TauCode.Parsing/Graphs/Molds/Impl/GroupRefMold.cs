using System;
using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    // todo regions
    public class GroupRefMold : PartMoldBase, IGroupRefMold
    {
        private readonly GroupRefEntranceVertexResolver _entrance;
        private readonly GroupRefExitVertexResolver _exit;

        public GroupRefMold(IGroupMold owner)
            : base(owner)
        {
            _entrance = new GroupRefEntranceVertexResolver();
        }

        public override string GetFullPath()
        {
            throw new NotImplementedException();
        }

        public override IVertexMold Entrance
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override IVertexMold Exit
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ReferencedGroupPath { get; set; }
    }
}
