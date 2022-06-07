using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    public class GroupMold : PartMoldBase, IGroupMold
    {
        #region Fields

        private readonly List<IPartMold> _parts;

        #endregion

        #region ctor

        public GroupMold(IGroupMold owner)
            : base(owner)
        {
            _parts = new List<IPartMold>();
        }

        #endregion

        #region IGroupMold Members

        public string Name { get; set; }
        public string FullPath => throw new NotImplementedException();
        public IReadOnlyList<IPartMold> Content => _parts;
        public void Add(IPartMold part)
        {
            // todo checks
            _parts.Add(part);
        }

        #endregion
    }
}
