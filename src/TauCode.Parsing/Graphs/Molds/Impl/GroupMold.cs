using System;
using System.Collections.Generic;
using System.Text;

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

        public string FullPath
        {
            get
            {
                if (this.Owner == null)
                {
                    if (this.Name == null)
                    {
                        return null;
                    }
                    else
                    {
                        return $"/{this.Name}/";
                    }
                }

                var ownerFullPath = this.Owner.FullPath;
                if (ownerFullPath == null)
                {
                    return null;
                }

                return $"{ownerFullPath}{this.Name}/";
            }
        }

        public IReadOnlyList<IPartMold> Content => _parts;
        public void Add(IPartMold part)
        {
            // todo checks
            _parts.Add(part);
        }

        #endregion

        public override IVertexMold Entrance { get; internal set; }
        public override IVertexMold Exit { get; internal set; }
    }
}
