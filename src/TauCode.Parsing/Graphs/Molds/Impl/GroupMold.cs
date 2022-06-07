using System;
using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.Graphs.Molds.Impl
{
    public class GroupMold : PartMoldBase, IGroupMold
    {
        #region Fields

        private readonly List<IPartMold> _parts;
        private string _name;
        private string _fullPath;

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
                if (_fullPath == null)
                {
                    _fullPath = this.BuildFullPath();
                }

                return _fullPath;
            }
        }

        private string BuildFullPath()
        {
            if (this.Owner == null)
            {
                return this.Name;
            }

            if (_name == null)
            {
                throw new NotImplementedException();
            }

            var sb = new StringBuilder();
            sb.Append(this.Owner.FullPath);
            sb.Append(this.Name);
            sb.Append("/");

            return sb.ToString();
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

        public override string Name
        {
            get
            {
                if (this.Owner == null)
                {
                    return "/";
                }

                return _name;
            }
            set
            {
                if (this.Owner == null)
                {
                    throw new NotImplementedException();
                }

                _fullPath = null;
                _name = value; // todo: check name is good
            }
        }
    }
}
